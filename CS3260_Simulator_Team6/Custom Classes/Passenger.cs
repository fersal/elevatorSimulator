using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS3260_Simulator_Team6
{
    public class Passenger
    {
        #region FIELDS

        private readonly object locker = new object();
        private Building myBuilding;
        private Floor currentFloor;
        private int currentFloorIndex;
        public Direction PassengerDirection;
        private PassengerStatus passengerStatus;
        private Floor targetFloor;
        private int targetFloorIndex;
        private bool visible;
        private int passengerAnimationDelay;
        private Elevator myElevator;
        private List<Image> fourthFloorPassengerImages;
        private List<Image> thirdFloorPassengerImages;
        private List<Image> secondFloorPassengerImages;
        private List<Image> firstFloorPassengerImages;
        private int index;
        private Image passengerImage;
        private static int passengerID;
        private readonly int instanceID;
        private MainWindow window;
        private DateTime StartTime, StopTime;
        Stopwatch stopWatch;
        Stopwatch TotalWatch = new Stopwatch();

        #endregion


        #region METHODS

        public Passenger(Building MyBuilding, Floor CurrentFloor, int TargetFloorIndex, Image passengerImage)
        {
            this.instanceID = ++passengerID;
            StartTime = DateTime.Now;
            stopWatch = Stopwatch.StartNew();
            TotalWatch.Start();
            window = (MainWindow)Application.Current.MainWindow;
            this.fourthFloorPassengerImages = new List<Image>();
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_1);
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_2);
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_3);
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_4);
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_5);
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_6);
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_7);
            this.fourthFloorPassengerImages.Add(window.imgElevatorFourPerson_8);
            this.thirdFloorPassengerImages = new List<Image>();
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_1);
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_2);
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_3);
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_4);
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_5);
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_6);
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_7);
            this.thirdFloorPassengerImages.Add(window.imgElevatorThreePerson_8);
            this.secondFloorPassengerImages = new List<Image>();
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_1);
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_2);
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_3);
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_4);
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_5);
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_6);
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_7);
            this.secondFloorPassengerImages.Add(window.imgElevatorTwoPerson_8);
            this.firstFloorPassengerImages = new List<Image>();
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_1);
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_2);
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_3);
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_4);
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_5);
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_6);
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_7);
            this.firstFloorPassengerImages.Add(window.imgElevatorOnePerson_8);
            this.myBuilding = MyBuilding;

            this.currentFloor = CurrentFloor;
            this.currentFloorIndex = CurrentFloor.FloorIndex;
            this.passengerStatus = PassengerStatus.WaitingForAnElevator;

            this.targetFloor = MyBuilding.ArrayOfAllFloors[TargetFloorIndex];
            this.targetFloorIndex = TargetFloorIndex;
            this.passengerImage = passengerImage;

            Random random = new Random();

            this.visible = true;
            this.passengerAnimationDelay = 10;

            //Subscribe to events
            this.currentFloor.NewPassengerAppeared += new EventHandler(currentFloor.Floor_NewPassengerAppeared);
            this.currentFloor.NewPassengerAppeared += new EventHandler(this.Passenger_NewPassengerAppeared);
            this.currentFloor.ElevatorHasArrivedOrIsNotFullAnymore += new EventHandler(this.Passenger_ElevatorHasArrivedOrIsNoteFullAnymore);
        }

        private void FindAnElevatorOrCallForANewOne()
        {
            UpdatePassengerDirection();

            //Copy the list of elevators available now on current floor
            List<Elevator> ListOfElevatorsWaitingOnMyFloor = currentFloor.GetListOfElevatorsWaitingHere();

            //Search the right elevator on my floor
            foreach (Elevator elevator in ListOfElevatorsWaitingOnMyFloor)
            {
                if (ElevatorsDirectionIsNoneOrOk(elevator))
                {
                    if (elevator.AddNewPassengerIfPossible(this, this.targetFloor))
                    {
                        //Update insideTheElevator
                        this.passengerStatus = PassengerStatus.GettingIntoTheElevator;

                        ThreadPool.QueueUserWorkItem(delegate { GetInToTheElevator(elevator); });
                        return;
                    }
                }
            }

            //Call for an elevator
            myBuilding.ElevatorManager.PassengerNeedsAnElevator(currentFloor, this.PassengerDirection);
        }

        private void GetInToTheElevator(Elevator ElevatorToGetIn)
        {
            //Rise an event
            ElevatorToGetIn.OnPassengerEnteredTheElevator(new PassengerEventArgs(this));

            //Unsubscribe from an event for current floor
            this.currentFloor.ElevatorHasArrivedOrIsNotFullAnymore -= this.Passenger_ElevatorHasArrivedOrIsNoteFullAnymore;

            //Move the picture on the UI
            RemovePassengerFromFloor();
            Thread.Sleep(this.passengerAnimationDelay);
            MovePassengersGraphicIn();

            //Update myElevator
            this.myElevator = ElevatorToGetIn;
        }

        public void ElevatorReachedNextFloor()
        {
            //For passengers, who are already inside an elevator:
            if (this.myElevator.GetCurrentFloor() == this.targetFloor)
            {
                //Set appropriate flag
                this.passengerStatus = PassengerStatus.LeavingTheBuilding;

                //Get out of the elevator
                ThreadPool.QueueUserWorkItem(delegate { GetOutOfTheElevator(this.myElevator); });
            }
        }

        private void GetOutOfTheElevator(Elevator ElevatorWhichArrived)
        {
            //Remove passenger from elevator
            ElevatorWhichArrived.RemovePassenger(this);

            //Leave the building
            this.LeaveTheBuilding();
        }

        private void UpdatePassengerDirection()
        {
            if (currentFloorIndex < targetFloorIndex)
            {
                this.PassengerDirection = Direction.Up;
            }
            else
            {
                this.PassengerDirection = Direction.Down;
            }
        }

        private bool ElevatorsDirectionIsNoneOrOk(Elevator ElevatorOnMyFloor)
        {
            //Check if elevator has more floors to visit            
            if (ElevatorOnMyFloor.GetElevatorDirection() == this.PassengerDirection)
            {
                return true; //Elevator direction is OK
            }
            else if (ElevatorOnMyFloor.GetElevatorDirection() == Direction.None)
            {
                return true; //If an elevator has no floors to visit, then it is always the right elevator
            }

            return false; //Elevator direction is NOT OK
        }

        private void LeaveTheBuilding()
        {
            int startFloor = this.currentFloorIndex + 1;
            int endFloor = this.targetFloorIndex + 1;
            //Move the passenger up to the exit
            MovePassengersGraphicOut();

            StopTime = DateTime.Now;
            stopWatch.Stop();
            TotalWatch.Stop();

            TimeSpan elapsed = StopTime.Subtract(StartTime);
            string passengerTravelTime = elapsed.TotalSeconds.ToString("00.00");
            App.Current.Dispatcher.Invoke((Action)delegate {
                window.listBoxPassengerLog.Items.Add(String.Format("Passenger {0}: Travel time from floor {1} to floor {2} was {3} seconds", instanceID, startFloor, endFloor, passengerTravelTime));
            });

            //No need to animate it
            myBuilding.ListOfAllPeopleWhoNeedAnimation.Remove(this);
        }

        private void MovePassengersGraphicOut()
        {

            if (currentFloorIndex == 3)
            {
                App.Current.Dispatcher.Invoke((Action)delegate {
                    thirdFloorPassengerImages[index].Opacity = 0;
                    thirdFloorPassengerImages[index].Source = null;
                    secondFloorPassengerImages[index].Opacity = 0;
                    secondFloorPassengerImages[index].Source = null;
                    firstFloorPassengerImages[index].Opacity = 0;
                    firstFloorPassengerImages[index].Source = null;

                    var animationRemoveFloorFour = new DoubleAnimation
                    {
                        From = 100,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(200),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationRemoveFloorFour.Completed += (s, a) => fourthFloorPassengerImages[index].Opacity = 0;
                    fourthFloorPassengerImages[index].BeginAnimation(UIElement.OpacityProperty, animationRemoveFloorFour);
                    fourthFloorPassengerImages[index].Source = null;
                    Thread.Sleep(this.passengerAnimationDelay);
                });
            }
            else if (currentFloorIndex == 2)
            {
                App.Current.Dispatcher.Invoke((Action)delegate {
                    fourthFloorPassengerImages[index].Opacity = 0;
                    fourthFloorPassengerImages[index].Source = null;
                    secondFloorPassengerImages[index].Opacity = 0;
                    secondFloorPassengerImages[index].Source = null;
                    firstFloorPassengerImages[index].Opacity = 0;
                    firstFloorPassengerImages[index].Source = null;

                    var animationRemoveFloorFour = new DoubleAnimation
                    {
                        From = 100,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(200),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationRemoveFloorFour.Completed += (s, a) => thirdFloorPassengerImages[index].Opacity = 0;
                    thirdFloorPassengerImages[index].BeginAnimation(UIElement.OpacityProperty, animationRemoveFloorFour);
                    thirdFloorPassengerImages[index].Source = null;
                    Thread.Sleep(this.passengerAnimationDelay);
                });
            }
            else if (currentFloorIndex == 1)
            {
                App.Current.Dispatcher.Invoke((Action)delegate {
                    fourthFloorPassengerImages[index].Opacity = 0;
                    fourthFloorPassengerImages[index].Source = null;
                    thirdFloorPassengerImages[index].Opacity = 0;
                    thirdFloorPassengerImages[index].Source = null;
                    firstFloorPassengerImages[index].Opacity = 0;
                    firstFloorPassengerImages[index].Source = null;

                    var animationRemoveFloorFour = new DoubleAnimation
                    {
                        From = 100,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(200),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationRemoveFloorFour.Completed += (s, a) => secondFloorPassengerImages[index].Opacity = 0;
                    secondFloorPassengerImages[index].BeginAnimation(UIElement.OpacityProperty, animationRemoveFloorFour);
                    secondFloorPassengerImages[index].Source = null;
                    Thread.Sleep(this.passengerAnimationDelay);
                });
            }
            else if (currentFloorIndex == 0)
            {
                App.Current.Dispatcher.Invoke((Action)delegate {
                    fourthFloorPassengerImages[index].Opacity = 0;
                    fourthFloorPassengerImages[index].Source = null;
                    thirdFloorPassengerImages[index].Opacity = 0;
                    thirdFloorPassengerImages[index].Source = null;
                    secondFloorPassengerImages[index].Opacity = 0;
                    secondFloorPassengerImages[index].Source = null;

                    var animationRemoveFloorFour = new DoubleAnimation
                    {
                        From = 100,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(200),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationRemoveFloorFour.Completed += (s, a) => firstFloorPassengerImages[index].Opacity = 0;
                    firstFloorPassengerImages[index].BeginAnimation(UIElement.OpacityProperty, animationRemoveFloorFour);
                    firstFloorPassengerImages[index].Source = null;
                    Thread.Sleep(this.passengerAnimationDelay);
                });
            }
        }

        private void RemovePassengerFromFloor()
        {
            App.Current.Dispatcher.Invoke((Action)delegate {

                var animationRemoveFloor = new DoubleAnimation
                {
                    From = 100,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(50),
                    FillBehavior = FillBehavior.Stop
                };
                animationRemoveFloor.Completed += (s, a) => passengerImage.Opacity = 0;
                passengerImage.BeginAnimation(UIElement.OpacityProperty, animationRemoveFloor);
                Thread.Sleep(this.passengerAnimationDelay);
            });
        }
        private void MovePassengersGraphicIn()
        {
            bool completed = false;
            if (currentFloorIndex == 3)
            {
                foreach (var person in fourthFloorPassengerImages)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        if (person.Source == null)
                        {
                            person.Source = passengerImage.Source;
                            index = fourthFloorPassengerImages.IndexOf(person);
                            thirdFloorPassengerImages[index].Source = passengerImage.Source;
                            thirdFloorPassengerImages[index].Opacity = 100;
                            secondFloorPassengerImages[index].Source = passengerImage.Source;
                            secondFloorPassengerImages[index].Opacity = 100;
                            firstFloorPassengerImages[index].Source = passengerImage.Source;
                            firstFloorPassengerImages[index].Opacity = 100;
                            var animationAddFloorFour = new DoubleAnimation
                            {
                                From = 0,
                                To = 100,
                                Duration = TimeSpan.FromMilliseconds(200),
                                FillBehavior = FillBehavior.Stop
                            };
                            animationAddFloorFour.Completed += (s, a) => person.Opacity = 100;
                            person.BeginAnimation(UIElement.OpacityProperty, animationAddFloorFour);
                            Thread.Sleep(this.passengerAnimationDelay);
                            completed = true;
                        }
                    });
                    if (completed)
                        break;
                }
            }
            else if (currentFloorIndex == 2)
            {
                foreach (var person in thirdFloorPassengerImages)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        if (person.Source == null)
                        {
                            person.Source = passengerImage.Source;
                            index = thirdFloorPassengerImages.IndexOf(person);
                            fourthFloorPassengerImages[index].Source = passengerImage.Source;
                            fourthFloorPassengerImages[index].Opacity = 100;
                            secondFloorPassengerImages[index].Source = passengerImage.Source;
                            secondFloorPassengerImages[index].Opacity = 100;
                            firstFloorPassengerImages[index].Source = passengerImage.Source;
                            firstFloorPassengerImages[index].Opacity = 100;
                            var animationAddFloorFour = new DoubleAnimation
                            {
                                From = 0,
                                To = 100,
                                Duration = TimeSpan.FromMilliseconds(200),
                                FillBehavior = FillBehavior.Stop
                            };
                            animationAddFloorFour.Completed += (s, a) => person.Opacity = 100;
                            person.BeginAnimation(UIElement.OpacityProperty, animationAddFloorFour);
                            Thread.Sleep(this.passengerAnimationDelay);
                            completed = true;
                        }
                    });
                    if (completed)
                        break;
                }
            }
            else if (currentFloorIndex == 1)
            {
                foreach (var person in secondFloorPassengerImages)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        if (person.Source == null)
                        {
                            person.Source = passengerImage.Source;
                            index = secondFloorPassengerImages.IndexOf(person);
                            fourthFloorPassengerImages[index].Source = passengerImage.Source;
                            fourthFloorPassengerImages[index].Opacity = 100;
                            thirdFloorPassengerImages[index].Source = passengerImage.Source;
                            thirdFloorPassengerImages[index].Opacity = 100;
                            firstFloorPassengerImages[index].Source = passengerImage.Source;
                            firstFloorPassengerImages[index].Opacity = 100;
                            var animationAddFloorFour = new DoubleAnimation
                            {
                                From = 0,
                                To = 100,
                                Duration = TimeSpan.FromMilliseconds(200),
                                FillBehavior = FillBehavior.Stop
                            };
                            animationAddFloorFour.Completed += (s, a) => person.Opacity = 100;
                            person.BeginAnimation(UIElement.OpacityProperty, animationAddFloorFour);
                            Thread.Sleep(this.passengerAnimationDelay);
                            completed = true;
                        }
                    });
                    if (completed)
                        break;
                }
            }
            else if (currentFloorIndex == 0)
            {
                foreach (var person in firstFloorPassengerImages)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        if (person.Source == null)
                        {
                            person.Source = passengerImage.Source;
                            index = firstFloorPassengerImages.IndexOf(person);
                            fourthFloorPassengerImages[index].Source = passengerImage.Source;
                            fourthFloorPassengerImages[index].Opacity = 100;
                            thirdFloorPassengerImages[index].Source = passengerImage.Source;
                            thirdFloorPassengerImages[index].Opacity = 100;
                            secondFloorPassengerImages[index].Source = passengerImage.Source;
                            secondFloorPassengerImages[index].Opacity = 100;
                            var animationAddFloorFour = new DoubleAnimation
                            {
                                From = 0,
                                To = 100,
                                Duration = TimeSpan.FromMilliseconds(200),
                                FillBehavior = FillBehavior.Stop
                            };
                            animationAddFloorFour.Completed += (s, a) => person.Opacity = 100;
                            person.BeginAnimation(UIElement.OpacityProperty, animationAddFloorFour);
                            Thread.Sleep(100);
                            completed = true;
                        }
                    });
                    if (completed)
                        break;
                }
            }


        }

        public Floor GetTargetFloor()
        {
            return this.targetFloor;
        }

        public bool GetPassengerVisibility()
        {
            return this.visible;
        }

        public int GetAnimationDelay()
        {
            return this.passengerAnimationDelay;
        }

        public int GetPickUpFloor()
        {
            return this.currentFloorIndex;
        }
        #endregion


        #region EVENT HANDLERS

        public void Passenger_NewPassengerAppeared(object sender, EventArgs e)
        {
            //Unsubscribe from this event (not needed anymore)            
            this.currentFloor.NewPassengerAppeared -= this.Passenger_NewPassengerAppeared;

            //Search an elevator
            FindAnElevatorOrCallForANewOne();
        }

        public void Passenger_ElevatorHasArrivedOrIsNoteFullAnymore(object sender, EventArgs e)
        {
            lock (locker) //Few elevators (on different threads) can rise this event at the same time
            {
                Elevator ElevatorWhichRisedAnEvent = ((ElevatorEventArgs)e).ElevatorWhichRisedAnEvent;

                //For passengers who are getting in to the elevator and may not be able to unsubscribe yet                
                if (this.passengerStatus == PassengerStatus.GettingIntoTheElevator)
                {
                    return;
                }

                //For passengers, who await for an elevator
                if (this.passengerStatus == PassengerStatus.WaitingForAnElevator)
                {
                    if ((ElevatorsDirectionIsNoneOrOk(ElevatorWhichRisedAnEvent) && (ElevatorWhichRisedAnEvent.AddNewPassengerIfPossible(this, targetFloor))))
                    {
                        //Set passengerStatus
                        passengerStatus = PassengerStatus.GettingIntoTheElevator;

                        //Get in to the elevator
                        ThreadPool.QueueUserWorkItem(delegate { GetInToTheElevator(ElevatorWhichRisedAnEvent); });
                    }
                    else
                    {
                        FindAnElevatorOrCallForANewOne();
                    }
                }
            }
        }

        #endregion EVENT HANDLERS
    }
}
