using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CS3260_Simulator_Team6
{
    public class Floor
    {
        #region FIELDS
        private readonly object locker = new object();
        private Building myBuilding;
        private int maximumAmmountOfPeopleInTheQueue; // depends on graphic
        private Passenger[] arrayOfPeopleWaitingForElevator;
        private List<Elevator> listOfElevatorsWaitingHere;
        private bool LampUp;
        private bool LampDown;
        private MainWindow window;
        private int floorIndex; //possible values for current graphic: 0, 1, 2, 3

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public int FloorIndex
        {
            get { return floorIndex; }
            private set { }
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public bool LampUpLight
        {
            get { return LampUp; }
            set
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    if (floorIndex == 2 && value == true)
                        window.floorThreeUpIndicator.Fill = Brushes.LightGreen;
                    else if (floorIndex == 2 && value == false)
                        window.floorThreeUpIndicator.Fill = Brushes.White;
                    else if (floorIndex == 1 && value == true)
                        window.floorTwoUpIndicator.Fill = Brushes.LightGreen;
                    else if (floorIndex == 1 && value == false)
                        window.floorTwoUpIndicator.Fill = Brushes.White;
                    else if (floorIndex == 0 && value == true)
                        window.floorOneUpIndicator.Fill = Brushes.LightGreen;
                    else if (floorIndex == 0 && value == false)
                        window.floorOneUpIndicator.Fill = Brushes.White;
                });
                LampUp = value;
            }
        } //indicates, that at least one of passengers wants to up

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public bool LampDownLight
        {
            get { return LampDown; }
            set
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    if (floorIndex == 3 && value == true)
                        window.floorFourDownIndicator.Fill = Brushes.LightGreen;
                    else if (floorIndex == 3 && value == false)
                        window.floorFourDownIndicator.Fill = Brushes.White;
                    else if (floorIndex == 2 && value == true)
                        window.floorThreeDownIndicator.Fill = Brushes.LightGreen;
                    else if (floorIndex == 2 && value == false)
                        window.floorThreeDownIndicator.Fill = Brushes.White;
                    else if (floorIndex == 1 && value == true)
                        window.floorTwoDownIndicator.Fill = Brushes.LightGreen;
                    else if (floorIndex == 1 && value == false)
                        window.floorTwoDownIndicator.Fill = Brushes.White;
                });
                LampDown = value;
            }
        } //indicates, that at least one of passengers wants to down
        #endregion
        #region METHODS

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public Floor(Building myBuilding, int floorNumber)
        {
            this.myBuilding = myBuilding;

            maximumAmmountOfPeopleInTheQueue = 8; //only 8 passengers at once can be visible in current layout
            this.arrayOfPeopleWaitingForElevator = new Passenger[maximumAmmountOfPeopleInTheQueue];
            this.floorIndex = floorNumber;

            listOfElevatorsWaitingHere = new List<Elevator>();
            window = (MainWindow)System.Windows.Application.Current.MainWindow;
            //Turn off both lamps
            LampUp = false;
            LampDown = false;
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        private int FindPassengerIndex(Passenger passenger)
        {
            for (int i = 0; i < maximumAmmountOfPeopleInTheQueue; i++)
            {
                if (arrayOfPeopleWaitingForElevator[i] != null)
                {
                    if (arrayOfPeopleWaitingForElevator[i].GetPickUpFloor().Equals(passenger.GetPickUpFloor()))
                    {
                        return i;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        private int? FindFirstFreeSlotInQueue()
        {
            //Lock not needed. Only one reference, already locked.
            for (int i = 0; i < maximumAmmountOfPeopleInTheQueue; i++)
            {
                if (arrayOfPeopleWaitingForElevator[i] == null)
                {
                    return i;
                }
            }

            return null;
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        private void AddRemoveNewPassengerToTheQueue(Passenger PassengerToAddOrRemvove, bool AddFlag)
        {
            //Lock not needed. Only two references (from this), both already locked                        
            if (AddFlag) //Add passenger
            {
                int? FirstFreeSlotInQueue = FindFirstFreeSlotInQueue(); //Make sure there is a space to add new passenger
                if (FirstFreeSlotInQueue != null)
                {
                    //Add passenger object to an array                    
                    this.arrayOfPeopleWaitingForElevator[(int)FirstFreeSlotInQueue] = PassengerToAddOrRemvove;

                    //Add passenger to Building's list
                    myBuilding.ListOfAllPeopleWhoNeedAnimation.Add(PassengerToAddOrRemvove);
                }
            }
            else //Remove passenger
            {
                int PassengerToRemoveIndex = FindPassengerIndex(PassengerToAddOrRemvove);
                if(PassengerToRemoveIndex >= 0)
                    this.GetArrayOfPeopleWaitingForElevator()[PassengerToRemoveIndex] = null;
            }
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public void AddRemoveElevatorToTheListOfElevatorsWaitingHere(Elevator ElevatorToAddOrRemove, bool AddFlag)
        {
            lock (locker) //Few elevators can try to add/remove themselfs at the same time
            {
                if (AddFlag) //Add elevator
                {
                    //Add elevator to the list
                    listOfElevatorsWaitingHere.Add(ElevatorToAddOrRemove);

                    //Subscribe to an event, rised when passenger entered the elevator
                    ElevatorToAddOrRemove.PassengerEnteredTheElevator += new EventHandler(this.Floor_PassengerEnteredTheElevator);
                }
                else //Remove elevator
                {
                    //Remove elevator from the list
                    listOfElevatorsWaitingHere.Remove(ElevatorToAddOrRemove);

                    //Unsubscribe from an event, rised when passenger entered the elevator
                    ElevatorToAddOrRemove.PassengerEnteredTheElevator -= this.Floor_PassengerEnteredTheElevator;
                }
            }
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public int GetMaximumAmmountOfPeopleInTheQueue()
        {
            return maximumAmmountOfPeopleInTheQueue;
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public int GetCurrentAmmountOfPeopleInTheQueue()
        {
            lock (locker) //The same lock is on add/remove passenger to the queue
            {
                int CurrentAmmountOfPeopleInTheQueue = 0;
                for (int i = 0; i < maximumAmmountOfPeopleInTheQueue; i++)
                {
                    if (this.arrayOfPeopleWaitingForElevator[i] != null)
                    {
                        CurrentAmmountOfPeopleInTheQueue++;
                    }
                }
                return CurrentAmmountOfPeopleInTheQueue;
            }
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public Passenger[] GetArrayOfPeopleWaitingForElevator()
        {
            return arrayOfPeopleWaitingForElevator;
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public List<Elevator> GetListOfElevatorsWaitingHere()
        {
            //Lock not needed. Method for passengers only.
            lock (locker)
            {
                return this.listOfElevatorsWaitingHere;
            }
        }
        #endregion
        #region EVENTS

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public event EventHandler NewPassengerAppeared;
        public void OnNewPassengerAppeared(EventArgs e)
        {
            EventHandler newPassengerAppeared = NewPassengerAppeared;
            if (newPassengerAppeared != null)
            {
                newPassengerAppeared(this, e);
            }
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public event EventHandler ElevatorHasArrivedOrIsNotFullAnymore;
        public void OnElevatorHasArrivedOrIsNoteFullAnymore(ElevatorEventArgs e)
        {
            EventHandler elevatorHasArrivedOrIsNoteFullAnymore = ElevatorHasArrivedOrIsNotFullAnymore;
            if (elevatorHasArrivedOrIsNoteFullAnymore != null)
            {
                elevatorHasArrivedOrIsNoteFullAnymore(this, e);
            }
        }
        #endregion
        #region EVENT HANDLERS
        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public void Floor_NewPassengerAppeared(object sender, EventArgs e)
        {
            lock (locker)
            {
                //Unsubscribe from this event (not needed anymore)
                this.NewPassengerAppeared -= this.Floor_NewPassengerAppeared;

                Passenger NewPassenger = ((PassengerEventArgs)e).PassengerWhoRisedAnEvent;

                AddRemoveNewPassengerToTheQueue(NewPassenger, true);
            }
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public void Floor_PassengerEnteredTheElevator(object sender, EventArgs e)
        {
            lock (locker)
            {
                Passenger PassengerWhoEnteredOrLeftTheElevator = ((PassengerEventArgs)e).PassengerWhoRisedAnEvent;

                //Remove passenger from queue                
                AddRemoveNewPassengerToTheQueue(PassengerWhoEnteredOrLeftTheElevator, false);
            }
        }
        #endregion
    }
}
