using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Diagnostics;

namespace CS3260_Simulator_Team6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region FIELDS
        private const int MAX_AUTO_PASSENGERS = 30;
        private bool playPause;
        private int prevFloorFourSelectedImg = 0;
        private int fourthFloorPassengerCount = 0;
        private int prevFloorThreeSelectedImg = 0;
        private int fourthFloorDownPassengerCount = 0;
        private int thirdFloorPassengerCount = 0;
        private int thirdFloorUpPassengerCount = 0;
        private int thirdFloorDownPassengerCount = 0;
        private int prevFloorTwoSelectedImg = 0;
        private int secondFloorPassengerCount = 0;
        private int secondFloorUpPassengerCount = 0;
        private int secondFloorDownPassengerCount = 0;
        private int prevFloorOneSelectedImg = 0;
        private int firstFloorPassengerCount = 0;
        private int firstFloorUpPassengerCount = 0;
        private const int MAX_PASSANGERS = 8;
        private int totalPassengerCountID = 0;
        private bool fourthFloorDownButtonClicked;
        private bool thirdFloorDownButtonClicked;
        private bool secondFloorDownButtonClicked;
        private bool thirdFloorUpButtonClicked;
        private bool secondFloorUpButtonClicked;
        private bool firstFloorUpButtonClicked;
        private bool AutoMode = false;
        private readonly object locker = new object();
        public Building MyBuilding;
        private AddNewPassenger AddPassenger = null;
        private WriteToFile WriteLog = null;
        private List<Image> fourthFloorPassengerImages = new List<Image>();
        private List<Image> fourthFloorSelectedPassengerImages = new List<Image>();
        private List<int> fourthFloorPassengerDestination = new List<int>();
        private List<Image> thirdFloorPassengerImages = new List<Image>();
        private List<Image> thirdFloorUpPassengerImages = new List<Image>();
        private List<int> thirdFloorUpPassengerDestination = new List<int>();
        private List<Image> secondFloorUpPassengerImages = new List<Image>();
        private List<int> secondFloorUpPassengerDestination = new List<int>();
        private List<Image> thirdFloorDownPassengerImages = new List<Image>();
        private List<int> thirdFloorDownPassengerDestination = new List<int>();
        private List<Image> secondFloorPassengerImages = new List<Image>();
        private List<Image> secondFloorDownPassengerImages = new List<Image>();
        private List<int> secondFloorDownPassengerDestination = new List<int>();
        private List<Image> firstFloorPassengerImages = new List<Image>();
        private List<Image> firstFloorSelectedPassengerImages = new List<Image>();
        private List<int> firstFloorPassengerDestination = new List<int>();
        private AControlsSystem command = null;
        private RequestPool request = null;
        private IReciever click = null;
        private FloorFirstUpCommand firstUpCmd = null;
        private FloorFourDownCommand fourDownCmd = null;
        private FloorThirdUpCommand thirdUpCmd = null;
        private FloorThirdDownCommand thirdDownCmd = null;
        private FloorSecondUpCommand secondUpCmd = null;
        private FloorSecondDownCommand secondDownCmd = null;
        private BrushConverter bc = new BrushConverter();
        private Doors doors = null;
        private MediaPlayer mplayer;
        FloorSelectionDialog dialog = null;
        Uri resourceUriDown = new Uri("resources/Arrow_Down.png", UriKind.Relative);
        Uri resourceUriDown_Clicked = new Uri("resources/Arrow_Down_Clicked.png", UriKind.Relative);
        Uri resourceUriUp = new Uri("resources/Arrow_Up.png", UriKind.Relative);
        Uri resourceUriUp_Clicked = new Uri("resources/Arrow_Up_Clicked.png", UriKind.Relative);
        #endregion

        #region MAINWINDOW INITIALIZER
        public MainWindow()
        {
            InitializeComponent();
            this.mplayer = new MediaPlayer();
            request = new RequestPool(listBoxRequestPool);
            click = new ButtonClick(request);
            firstUpCmd = new FloorFirstUpCommand(click);
            fourDownCmd = new FloorFourDownCommand(click);
            fourDownCmd = new FloorFourDownCommand(click);
            thirdUpCmd = new FloorThirdUpCommand(click);
            thirdDownCmd = new FloorThirdDownCommand(click);
            secondUpCmd = new FloorSecondUpCommand(click);
            secondDownCmd = new FloorSecondDownCommand(click);
            doors = new Doors();
            MyBuilding = new Building(doors);
            WriteLog = new WriteToFile();
            AddPassenger = new AddNewPassenger(WriteLog);
            this.doors.DoorsHaveOpened += new EventHandler(this.Doors_DoorsHaveOpened);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_1);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_2);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_3);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_4);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_5);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_6);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_7);
            this.fourthFloorPassengerImages.Add(imgFloorFourPerson_8);

            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_1);
            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_2);
            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_3);
            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_4);
            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_5);
            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_6);
            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_7);
            this.thirdFloorPassengerImages.Add(imgFloorThreePerson_8);

            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_1);
            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_2);
            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_3);
            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_4);
            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_5);
            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_6);
            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_7);
            this.secondFloorPassengerImages.Add(imgFloorTwoPerson_8);

            this.firstFloorPassengerImages.Add(imgFloorOnePerson_1);
            this.firstFloorPassengerImages.Add(imgFloorOnePerson_2);
            this.firstFloorPassengerImages.Add(imgFloorOnePerson_3);
            this.firstFloorPassengerImages.Add(imgFloorOnePerson_4);
            this.firstFloorPassengerImages.Add(imgFloorOnePerson_5);
            this.firstFloorPassengerImages.Add(imgFloorOnePerson_6);
            this.firstFloorPassengerImages.Add(imgFloorOnePerson_7);
            this.firstFloorPassengerImages.Add(imgFloorOnePerson_8);
            mplayer.Open(new Uri("Elevator_Music_1.mp3", UriKind.Relative));
            mplayer.MediaEnded += new EventHandler(Media_Ended);
            mplayer.Play();
        }

        #endregion

        #region METHODS
        

        private int GetFloorPassengerCount(List<Image> passengers)
        {
            int result = 0;
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < MAX_PASSANGERS; i++)
                {
                    if (passengers[i].Opacity == 100)
                    {
                        result++;
                    }
                }
            });

            return result;
        }

        private Image FloorFourAddPeople()
        {
            int peopleSelected = 0;
            Image selectedImg = null;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < MAX_PASSANGERS; i++)
                {
                    if (fourthFloorPassengerImages[i].Opacity == 0)
                    {
                        peopleSelected = i + 1;
                        break;
                    }
                }
            });

            switch (peopleSelected)
            {
                case 1:
                    selectedImg = imgFloorFourPerson_1;
                    break;
                case 2:
                    selectedImg = imgFloorFourPerson_2;
                    break;
                case 3:
                    selectedImg = imgFloorFourPerson_3;
                    break;
                case 4:
                    selectedImg = imgFloorFourPerson_4;
                    break;
                case 5:
                    selectedImg = imgFloorFourPerson_5;
                    break;
                case 6:
                    selectedImg = imgFloorFourPerson_6;
                    break;
                case 7:
                    selectedImg = imgFloorFourPerson_7;
                    break;
                case 8:
                    selectedImg = imgFloorFourPerson_8;
                    break;
            }
            if (selectedImg != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    var animationAddFloorFour = new DoubleAnimation
                    {
                        From = 0,
                        To = 100,
                        Duration = TimeSpan.FromMilliseconds(100),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationAddFloorFour.Completed += (s, a) => selectedImg.Opacity = 100;
                    selectedImg.BeginAnimation(UIElement.OpacityProperty, animationAddFloorFour);
                    prevFloorFourSelectedImg = peopleSelected;
                });

            }
            return selectedImg;
        }

        private Image FloorThreeAddPeople()
        {
            int peopleSelected = 0;
            Image selectedImg = null;
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < MAX_PASSANGERS; i++)
                {
                    if (thirdFloorPassengerImages[i].Opacity == 0)
                    {
                        peopleSelected = i + 1;
                        break;
                    }
                }
            });

            switch (peopleSelected)
            {
                case 1:
                    selectedImg = imgFloorThreePerson_1;
                    break;
                case 2:
                    selectedImg = imgFloorThreePerson_2;
                    break;
                case 3:
                    selectedImg = imgFloorThreePerson_3;
                    break;
                case 4:
                    selectedImg = imgFloorThreePerson_4;
                    break;
                case 5:
                    selectedImg = imgFloorThreePerson_5;
                    break;
                case 6:
                    selectedImg = imgFloorThreePerson_6;
                    break;
                case 7:
                    selectedImg = imgFloorThreePerson_7;
                    break;
                case 8:
                    selectedImg = imgFloorThreePerson_8;
                    break;
            }
            if (selectedImg != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    var animationAddFloorThree = new DoubleAnimation
                    {
                        From = 0,
                        To = 100,
                        Duration = TimeSpan.FromMilliseconds(100),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationAddFloorThree.Completed += (s, a) => selectedImg.Opacity = 100;
                    selectedImg.BeginAnimation(UIElement.OpacityProperty, animationAddFloorThree);
                    prevFloorThreeSelectedImg = peopleSelected;
                });
            }
            return selectedImg;
        }

        private Image FloorTwoAddPeople()
        {
            int peopleSelected = 0;
            Image selectedImg = null;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < MAX_PASSANGERS; i++)
                {
                    if (secondFloorPassengerImages[i].Opacity == 0)
                    {
                        peopleSelected = i + 1;
                        break;
                    }
                }
            });
            switch (peopleSelected)
            {
                case 1:
                    selectedImg = imgFloorTwoPerson_1;
                    break;
                case 2:
                    selectedImg = imgFloorTwoPerson_2;
                    break;
                case 3:
                    selectedImg = imgFloorTwoPerson_3;
                    break;
                case 4:
                    selectedImg = imgFloorTwoPerson_4;
                    break;
                case 5:
                    selectedImg = imgFloorTwoPerson_5;
                    break;
                case 6:
                    selectedImg = imgFloorTwoPerson_6;
                    break;
                case 7:
                    selectedImg = imgFloorTwoPerson_7;
                    break;
                case 8:
                    selectedImg = imgFloorTwoPerson_8;
                    break;
            }
            if (selectedImg != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    var animationAddFloorTwo = new DoubleAnimation
                    {
                        From = 0,
                        To = 100,
                        Duration = TimeSpan.FromMilliseconds(100),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationAddFloorTwo.Completed += (s, a) => selectedImg.Opacity = 100;
                    selectedImg.BeginAnimation(UIElement.OpacityProperty, animationAddFloorTwo);
                    prevFloorTwoSelectedImg = peopleSelected;
                });
            }
            return selectedImg;
        }

        private Image FloorOneAddPeople()
        {
            int peopleSelected = 0;
            Image selectedImg = null;
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < MAX_PASSANGERS; i++)
                {
                    if (firstFloorPassengerImages[i].Opacity == 0)
                    {
                        peopleSelected = i + 1;
                        break;
                    }
                }
            });
            switch (peopleSelected)
            {
                case 1:
                    selectedImg = imgFloorOnePerson_1;
                    break;
                case 2:
                    selectedImg = imgFloorOnePerson_2;
                    break;
                case 3:
                    selectedImg = imgFloorOnePerson_3;
                    break;
                case 4:
                    selectedImg = imgFloorOnePerson_4;
                    break;
                case 5:
                    selectedImg = imgFloorOnePerson_5;
                    break;
                case 6:
                    selectedImg = imgFloorOnePerson_6;
                    break;
                case 7:
                    selectedImg = imgFloorOnePerson_7;
                    break;
                case 8:
                    selectedImg = imgFloorOnePerson_8;
                    break;
            }
            if (selectedImg != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    var animationAddFloorOne = new DoubleAnimation
                    {
                        From = 0,
                        To = 100,
                        Duration = TimeSpan.FromMilliseconds(100),
                        FillBehavior = FillBehavior.Stop
                    };
                    animationAddFloorOne.Completed += (s, a) => selectedImg.Opacity = 100;
                    selectedImg.BeginAnimation(UIElement.OpacityProperty, animationAddFloorOne);
                    prevFloorOneSelectedImg = peopleSelected;
                });
            }
            return selectedImg;
        }

        private void CreateAutoPassenger(int count, int floor, int destination, Image pic)
        {
            lock (locker)
            {
                AddPassenger.FloorIndex = floor;
                AddPassenger.AddPassenger(destination, pic);
                AddPassenger.LoadPassenger();
            }
        }

        private void AddAutoPassenger()
        {
            Random rnd = new Random();
            int destination = 0;
            int floorIndex = 0;
            for (int i = 0; i < MAX_AUTO_PASSENGERS; i++)
            {
                int currentFloor = rnd.Next(1, 5);
                int waitTime = rnd.Next(0, 15000);
                if (currentFloor == 4)
                {
                    fourthFloorPassengerCount = GetFloorPassengerCount(fourthFloorPassengerImages);
                    if (fourthFloorPassengerCount <= MAX_PASSANGERS)
                    {
                        fourthFloorDownButtonClicked = true;
                        floorIndex = 3;
                        Image selectedImage = FloorFourAddPeople();
                        do
                            destination = rnd.Next(0, 4);
                        while (destination == floorIndex);
                        fourthFloorDownPassengerCount++;
                        Thread fourthFloorThread = new Thread(() => CreateAutoPassenger(fourthFloorPassengerCount, currentFloor - 1, destination, selectedImage));
                        fourthFloorThread.Start();
                    }
                    else
                    {
                        i--;
                    }
                }
                else if (currentFloor == 3)
                {
                    thirdFloorPassengerCount = GetFloorPassengerCount(thirdFloorPassengerImages);
                    if (thirdFloorPassengerCount <= MAX_PASSANGERS)
                    {
                        floorIndex = 2;
                        Image selectedImage = FloorThreeAddPeople();
                        do
                            destination = rnd.Next(0, 4);
                        while (destination == floorIndex);
                        if (destination > floorIndex)
                        {
                            thirdFloorUpButtonClicked = true;
                            thirdFloorUpPassengerCount++;
                            Thread thirdFloorUpThread = new Thread(() => CreateAutoPassenger(thirdFloorUpPassengerCount, currentFloor - 1, destination, selectedImage));
                            thirdFloorUpThread.Start();
                        }
                        else if (destination < floorIndex)
                        {
                            thirdFloorDownButtonClicked = true;
                            thirdFloorDownPassengerCount++;
                            Thread thirdFloorDownFloorThread = new Thread(() => CreateAutoPassenger(thirdFloorDownPassengerCount, currentFloor - 1, destination, selectedImage));
                            thirdFloorDownFloorThread.Start();
                        }
                    }
                    else
                    {
                        i--;
                    }

                }
                else if (currentFloor == 2)
                {
                    secondFloorPassengerCount = GetFloorPassengerCount(secondFloorPassengerImages);
                    if (secondFloorPassengerCount <= MAX_PASSANGERS)
                    {
                        floorIndex = 1;
                        Image selectedImage = FloorTwoAddPeople();
                        do
                            destination = rnd.Next(0, 3);
                        while (destination == floorIndex);
                        if (destination > floorIndex)
                        {
                            secondFloorUpButtonClicked = true;
                            secondFloorUpPassengerCount++;
                            Thread secondFloorUpThread = new Thread(() => CreateAutoPassenger(secondFloorUpPassengerCount, currentFloor - 1, destination, selectedImage));
                            secondFloorUpThread.Start();
                        }
                        else if (destination < floorIndex)
                        {
                            secondFloorDownButtonClicked = true;
                            secondFloorDownPassengerCount++;
                            Thread secondFloorDownFloorThread = new Thread(() => CreateAutoPassenger(secondFloorDownPassengerCount, currentFloor - 1, destination, selectedImage));
                            secondFloorDownFloorThread.Start();
                        }
                    }
                    else
                    {
                        i--;
                    }

                }
                else if (currentFloor == 1)
                {
                    firstFloorPassengerCount = GetFloorPassengerCount(firstFloorPassengerImages);
                    if (firstFloorPassengerCount <= MAX_PASSANGERS)
                    {
                        firstFloorUpButtonClicked = true;
                        floorIndex = 0;
                        Image selectedImage = FloorOneAddPeople();
                        do
                            destination = rnd.Next(0, 4);
                        while (destination == floorIndex);
                        firstFloorUpPassengerCount++;
                        Thread firstFloorUpThread = new Thread(() => CreateAutoPassenger(firstFloorUpPassengerCount, currentFloor - 1, destination, selectedImage));
                        firstFloorUpThread.Start();
                    }
                    else
                    {
                        i--;
                    }

                }
                Thread.Sleep(waitTime);
            }

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                listBoxRequestPool.Items.Remove("Auto processing 30 passengers.");
                sliderManualAuto.IsEnabled = true;
                StartButton.IsEnabled = true;
            });
        }
        #endregion

        #region EVENT HANDLERS
        public void Doors_DoorsHaveOpened(object sender, EventArgs e)
        {
            int floor = doors.CurrentFloor;
            string direction = doors.ElevatorDirection;
            if (floor == 3)
            {

                if (fourthFloorDownButtonClicked)
                {
                    if (!AutoMode)
                    {
                        command = fourDownCmd;
                        command.UnExecute();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriDown);
                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush();
                        brush.ImageSource = temp;
                        btnFourthFloorDown.Background = brush;
                    }
                    else
                    {
                        fourthFloorDownPassengerCount = 0;
                    }
                    fourthFloorPassengerCount = GetFloorPassengerCount(fourthFloorPassengerImages);
                    fourthFloorDownButtonClicked = false;
                }
            }
            else if (floor == 2)
            {
                if (direction == "Down")
                {
                    if (thirdFloorDownButtonClicked)
                    {
                        if (!AutoMode)
                        {
                            command = thirdDownCmd;
                            command.UnExecute();
                            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriDown);
                            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                            var brush = new ImageBrush();
                            brush.ImageSource = temp;
                            btnThirdFloorDown.Background = brush;
                        }
                        else
                        {
                            thirdFloorDownPassengerCount = 0;
                        }
                        thirdFloorPassengerCount = GetFloorPassengerCount(thirdFloorPassengerImages);
                        thirdFloorDownButtonClicked = false;
                    }
                }
                else if (direction == "Up")
                {
                    if (thirdFloorUpButtonClicked)
                    {
                        if (!AutoMode)
                        {
                            command = thirdUpCmd;
                            command.UnExecute();
                            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriUp);
                            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                            var brush = new ImageBrush();
                            brush.ImageSource = temp;
                            btnThirdFloorUp.Background = brush;
                        }
                        else
                        {
                            thirdFloorUpPassengerCount = 0;
                        }
                        thirdFloorPassengerCount = GetFloorPassengerCount(thirdFloorPassengerImages);
                        thirdFloorUpButtonClicked = false;
                    }
                }
            }
            else if (floor == 1)
            {

                if (direction == "Down")
                {
                    if (secondFloorDownButtonClicked)
                    {
                        if (!AutoMode)
                        {
                            command = secondDownCmd;
                            command.UnExecute();
                            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriDown);
                            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                            var brush = new ImageBrush();
                            brush.ImageSource = temp;
                            btnSecondFloorDown.Background = brush;
                        }
                        else
                        {
                            secondFloorDownPassengerCount = 0;
                        }
                        secondFloorPassengerCount = GetFloorPassengerCount(secondFloorPassengerImages);
                        secondFloorDownButtonClicked = false;
                    }
                }
                else if (direction == "Up")
                {
                    if (secondFloorUpButtonClicked)
                    {
                        if (!AutoMode)
                        {
                            command = secondUpCmd;
                            command.UnExecute();
                            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriUp);
                            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                            var brush = new ImageBrush();
                            brush.ImageSource = temp;
                            btnSecondFloorUp.Background = brush;
                        }
                        else
                        {
                            secondFloorUpPassengerCount = 0;
                        }
                        secondFloorPassengerCount = GetFloorPassengerCount(secondFloorPassengerImages);
                        secondFloorUpButtonClicked = false;
                    }
                }
            }
            else if (floor == 0)
            {

                if (firstFloorUpButtonClicked)
                {
                    if (!AutoMode)
                    {
                        command = firstUpCmd;
                        command.UnExecute();
                        StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriUp);
                        BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                        var brush = new ImageBrush();
                        brush.ImageSource = temp;
                        btnFirstFloorUp.Background = brush;
                    }
                    else
                    {
                        firstFloorUpPassengerCount = 0;
                    }
                    firstFloorPassengerCount = GetFloorPassengerCount(firstFloorPassengerImages);
                    firstFloorUpButtonClicked = false;
                }
            }
        }

        private void btnFourthFloorDown_Click(object sender, RoutedEventArgs e)
        {
            if (fourthFloorDownPassengerCount != 0)
            {
                command = fourDownCmd;
                fourthFloorDownButtonClicked = command.Execute();
                if (!fourthFloorDownButtonClicked)
                {
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriDown_Clicked);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    btnFourthFloorDown.Background = brush;
                    fourthFloorDownButtonClicked = true;
                }
                AddPassenger.FloorIndex = 3;
                for (int i = 0; i < fourthFloorDownPassengerCount; i++)
                {
                    AddPassenger.AddPassenger(fourthFloorPassengerDestination[i], fourthFloorSelectedPassengerImages[i]);
                }
                fourthFloorSelectedPassengerImages.Clear();
                fourthFloorPassengerDestination.Clear();
                AddPassenger.LoadPassenger();
                fourthFloorDownPassengerCount = 0;
            }
            else
            {
                MessageBox.Show("Please add passengers first.");
            }
            sliderManualAuto.Focus();
        }

        private void btnThirdFloorUp_Click(object sender, RoutedEventArgs e)
        {
            if (thirdFloorUpPassengerCount != 0)
            {
                command = thirdUpCmd;
                thirdFloorUpButtonClicked = command.Execute();
                if (!thirdFloorUpButtonClicked)
                {
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriUp_Clicked);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    btnThirdFloorUp.Background = brush;
                    thirdFloorUpButtonClicked = true;
                }
                AddPassenger.FloorIndex = 2;
                for (int i = 0; i < thirdFloorUpPassengerCount; i++)
                {
                    if (thirdFloorUpPassengerDestination[i] > 2)
                        AddPassenger.AddPassenger(thirdFloorUpPassengerDestination[i], thirdFloorUpPassengerImages[i]);
                }
                thirdFloorUpPassengerImages.Clear();
                thirdFloorUpPassengerDestination.Clear();
                AddPassenger.LoadPassenger();
                thirdFloorUpPassengerCount = 0;
            }
            else
            {
                MessageBox.Show("Please add passengers to go up first.");
            }
            sliderManualAuto.Focus();
        }

        private void btnThirdFloorDown_Click(object sender, RoutedEventArgs e)
        {
            if (thirdFloorDownPassengerCount != 0)
            {
                command = thirdDownCmd;
                thirdFloorDownButtonClicked = command.Execute();
                if (!thirdFloorDownButtonClicked)
                {
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriDown_Clicked);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    btnThirdFloorDown.Background = brush;
                    thirdFloorDownButtonClicked = true;
                }
                AddPassenger.FloorIndex = 2;
                for (int i = 0; i < thirdFloorDownPassengerCount; i++)
                {
                    if (thirdFloorDownPassengerDestination[i] < 2)
                        AddPassenger.AddPassenger(thirdFloorDownPassengerDestination[i], thirdFloorDownPassengerImages[i]);
                }
                thirdFloorDownPassengerImages.Clear();
                thirdFloorDownPassengerDestination.Clear();
                AddPassenger.LoadPassenger();
                thirdFloorDownPassengerCount = 0;
            }
            else
            {
                MessageBox.Show("Please add passengers to go down first.");
            }
            sliderManualAuto.Focus();
        }

        private void btnSecondFloorUp_Click(object sender, RoutedEventArgs e)
        {
            if (secondFloorUpPassengerCount != 0)
            {
                command = secondUpCmd;
                secondFloorUpButtonClicked = command.Execute();
                if (!secondFloorUpButtonClicked)
                {
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriUp_Clicked);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    btnSecondFloorUp.Background = brush;
                    secondFloorUpButtonClicked = true;
                }
                AddPassenger.FloorIndex = 1;
                for (int i = 0; i < secondFloorUpPassengerCount; i++)
                {
                    if (secondFloorUpPassengerDestination[i] > 1)
                        AddPassenger.AddPassenger(secondFloorUpPassengerDestination[i], secondFloorUpPassengerImages[i]);
                }
                secondFloorUpPassengerImages.Clear();
                secondFloorUpPassengerDestination.Clear();
                AddPassenger.LoadPassenger();
                secondFloorUpPassengerCount = 0;
            }
            else
            {
                MessageBox.Show("Please add passengers to go up first.");
            }
            sliderManualAuto.Focus();
        }

        private void btnSecondFloorDown_Click(object sender, RoutedEventArgs e)
        {
            if (secondFloorDownPassengerCount != 0)
            {
                command = secondDownCmd;
                secondFloorDownButtonClicked = command.Execute();
                if (!secondFloorDownButtonClicked)
                {
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriDown_Clicked);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    btnSecondFloorDown.Background = brush;
                    secondFloorDownButtonClicked = true;
                }
                AddPassenger.FloorIndex = 1;
                for (int i = 0; i < secondFloorDownPassengerCount; i++)
                {
                    if (secondFloorDownPassengerDestination[i] < 1)
                        AddPassenger.AddPassenger(secondFloorDownPassengerDestination[i], secondFloorDownPassengerImages[i]);
                }
                secondFloorDownPassengerImages.Clear();
                secondFloorDownPassengerDestination.Clear();
                AddPassenger.LoadPassenger();
                secondFloorDownPassengerCount = 0;
            }
            else
            {
                MessageBox.Show("Please add passengers to go down first.");
            }
            sliderManualAuto.Focus();
        }

        private void btnFirstFloorUp_Click(object sender, RoutedEventArgs e)
        {
            if (firstFloorUpPassengerCount != 0)
            {
                command = firstUpCmd;
                firstFloorUpButtonClicked = command.Execute();
                if (!firstFloorUpButtonClicked)
                {
                    StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUriUp_Clicked);
                    BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                    var brush = new ImageBrush();
                    brush.ImageSource = temp;
                    btnFirstFloorUp.Background = brush;
                    firstFloorUpButtonClicked = true;
                }
                AddPassenger.FloorIndex = 0;
                for (int i = 0; i < firstFloorUpPassengerCount; i++)
                {
                    AddPassenger.AddPassenger(firstFloorPassengerDestination[i], firstFloorSelectedPassengerImages[i]);
                }
                firstFloorSelectedPassengerImages.Clear();
                firstFloorPassengerDestination.Clear();
                AddPassenger.LoadPassenger();
                firstFloorUpPassengerCount = 0;
            }
            else
            {
                MessageBox.Show("Please add passengers first.");
            }
            sliderManualAuto.Focus();
        }

        private void btnAddPassengerFloorFour_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 4;
            fourthFloorPassengerCount = GetFloorPassengerCount(fourthFloorPassengerImages);
            if (fourthFloorPassengerCount <= MAX_PASSANGERS)
            {
                fourthFloorDownPassengerCount++;
                fourthFloorSelectedPassengerImages.Add(FloorFourAddPeople());
                FloorSelectionDialog dialog = new FloorSelectionDialog();
                for (int i = 1; i <= MyBuilding.ArrayOfAllFloors.Length; i++)
                {
                    if (i != floorIndex)
                    {
                        dialog.ListOfFloorsInComboBox.Add(i);
                    }
                }
                dialog.ShowDialog();
                int selectedFloor = (dialog.GetSelectedItem() - 1);
                fourthFloorPassengerDestination.Add(selectedFloor);
                if (fourthFloorDownButtonClicked)
                {
                    AddPassenger.FloorIndex = 3;
                    for (int i = 0; i < fourthFloorDownPassengerCount; i++)
                    {
                        AddPassenger.AddPassenger(fourthFloorPassengerDestination[i], fourthFloorSelectedPassengerImages[i]);
                    }
                    fourthFloorSelectedPassengerImages.Clear();
                    fourthFloorPassengerDestination.Clear();
                    fourthFloorDownPassengerCount = 0;
                    AddPassenger.LoadPassenger();
                }
                totalPassengerCountID++;
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
            }
        }

        private void btnAddPassengerFloorThree_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 3;
            thirdFloorPassengerCount = GetFloorPassengerCount(thirdFloorPassengerImages);
            if (thirdFloorPassengerCount <= MAX_PASSANGERS)
            {
                Image selectedImage = FloorThreeAddPeople();
                FloorSelectionDialog dialog = new FloorSelectionDialog();
                for (int i = 1; i <= MyBuilding.ArrayOfAllFloors.Length; i++)
                {
                    if (i != floorIndex)
                    {
                        dialog.ListOfFloorsInComboBox.Add(i);
                    }
                }
                dialog.ShowDialog();
                int selectedFloor = (dialog.GetSelectedItem() - 1);
                if (selectedFloor > 2)
                {
                    thirdFloorUpPassengerDestination.Add(selectedFloor);
                    thirdFloorUpPassengerImages.Add(selectedImage);
                    thirdFloorUpPassengerCount++;
                    if (thirdFloorUpButtonClicked)
                    {
                        AddPassenger.FloorIndex = 2;
                        for (int i = 0; i < thirdFloorUpPassengerCount; i++)
                        {
                            if (thirdFloorUpPassengerDestination[i] > 2)
                                AddPassenger.AddPassenger(thirdFloorUpPassengerDestination[i], thirdFloorUpPassengerImages[i]);
                        }
                        thirdFloorUpPassengerImages.Clear();
                        thirdFloorUpPassengerDestination.Clear();
                        thirdFloorUpPassengerCount = 0;
                        AddPassenger.LoadPassenger();
                    }
                }
                else if (selectedFloor < 2)
                {
                    thirdFloorDownPassengerDestination.Add(selectedFloor);
                    thirdFloorDownPassengerImages.Add(selectedImage);
                    thirdFloorDownPassengerCount++;
                    if (thirdFloorDownButtonClicked)
                    {
                        AddPassenger.FloorIndex = 2;
                        for (int i = 0; i < thirdFloorDownPassengerCount; i++)
                        {
                            if (thirdFloorDownPassengerDestination[i] < 2)
                                AddPassenger.AddPassenger(thirdFloorDownPassengerDestination[i], thirdFloorDownPassengerImages[i]);
                        }
                        thirdFloorDownPassengerImages.Clear();
                        thirdFloorDownPassengerDestination.Clear();
                        thirdFloorDownPassengerCount = 0;
                        AddPassenger.LoadPassenger();
                    }
                }

                totalPassengerCountID++;
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
            }

        }

        private void btnAddPassengerFloorTwo_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 2;
            secondFloorPassengerCount = GetFloorPassengerCount(secondFloorPassengerImages);
            if (secondFloorPassengerCount <= MAX_PASSANGERS)
            {
                Image selectedImage = FloorTwoAddPeople();
                FloorSelectionDialog dialog = new FloorSelectionDialog();
                for (int i = 1; i <= MyBuilding.ArrayOfAllFloors.Length; i++)
                {
                    if (i != floorIndex)
                    {
                        dialog.ListOfFloorsInComboBox.Add(i);
                    }
                }
                dialog.ShowDialog();
                int selectedFloor = (dialog.GetSelectedItem() - 1);
                if (selectedFloor > 1)
                {
                    secondFloorUpPassengerDestination.Add(selectedFloor);
                    secondFloorUpPassengerImages.Add(selectedImage);
                    secondFloorUpPassengerCount++;
                    if (secondFloorUpButtonClicked)
                    {
                        AddPassenger.FloorIndex = 1;
                        for (int i = 0; i < secondFloorUpPassengerCount; i++)
                        {
                            if (secondFloorUpPassengerDestination[i] > 1)
                                AddPassenger.AddPassenger(secondFloorUpPassengerDestination[i], secondFloorUpPassengerImages[i]);
                        }
                        secondFloorUpPassengerImages.Clear();
                        secondFloorUpPassengerDestination.Clear();
                        secondFloorUpPassengerCount = 0;
                        AddPassenger.LoadPassenger();
                    }
                }
                else if (selectedFloor < 1)
                {
                    secondFloorDownPassengerDestination.Add(selectedFloor);
                    secondFloorDownPassengerImages.Add(selectedImage);
                    secondFloorDownPassengerCount++;
                    if (secondFloorDownButtonClicked)
                    {
                        AddPassenger.FloorIndex = 1;
                        for (int i = 0; i < secondFloorDownPassengerCount; i++)
                        {
                            if (secondFloorDownPassengerDestination[i] < 1)
                                AddPassenger.AddPassenger(secondFloorDownPassengerDestination[i], secondFloorDownPassengerImages[i]);
                        }
                        secondFloorDownPassengerImages.Clear();
                        secondFloorDownPassengerDestination.Clear();
                        secondFloorDownPassengerCount = 0;
                        AddPassenger.LoadPassenger();
                    }
                }

                totalPassengerCountID++;
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
            }
        }

        private void btnAddPassengerFloorOne_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 1;
            firstFloorPassengerCount = GetFloorPassengerCount(firstFloorPassengerImages);
            if (firstFloorPassengerCount <= MAX_PASSANGERS)
            {
                firstFloorUpPassengerCount++;
                firstFloorSelectedPassengerImages.Add(FloorOneAddPeople());
                FloorSelectionDialog dialog = new FloorSelectionDialog();
                for (int i = 1; i <= MyBuilding.ArrayOfAllFloors.Length; i++)
                {
                    if (i != floorIndex)
                    {
                        dialog.ListOfFloorsInComboBox.Add(i);
                    }
                }
                dialog.ShowDialog();
                int selectedFloor = (dialog.GetSelectedItem() - 1);
                firstFloorPassengerDestination.Add(selectedFloor);
                totalPassengerCountID++;
                if (firstFloorUpButtonClicked)
                {
                    AddPassenger.FloorIndex = 0;
                    for (int i = 0; i < firstFloorUpPassengerCount; i++)
                    {
                        AddPassenger.AddPassenger(firstFloorPassengerDestination[i], firstFloorSelectedPassengerImages[i]);
                    }
                    firstFloorSelectedPassengerImages.Clear();
                    firstFloorPassengerDestination.Clear();
                    firstFloorUpPassengerCount = 0;
                    AddPassenger.LoadPassenger();
                }
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
            }
        }

        private void sliderManualAuto_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderManualAuto.Value == 1)
            {
                btnAddPassengerFloorFour.IsEnabled = false;
                btnAddPassengerFloorOne.IsEnabled = false;
                btnAddPassengerFloorThree.IsEnabled = false;
                btnAddPassengerFloorTwo.IsEnabled = false;
                btnFirstFloorUp.IsEnabled = false;
                btnSecondFloorDown.IsEnabled = false;
                btnSecondFloorUp.IsEnabled = false;
                btnThirdFloorDown.IsEnabled = false;
                btnThirdFloorUp.IsEnabled = false;
                btnFourthFloorDown.IsEnabled = false;
                StartButton.IsEnabled = true;
                AutoMode = true;
            }
            else
            {
                btnAddPassengerFloorFour.IsEnabled = true;
                btnAddPassengerFloorOne.IsEnabled = true;
                btnAddPassengerFloorThree.IsEnabled = true;
                btnAddPassengerFloorTwo.IsEnabled = true;
                btnFirstFloorUp.IsEnabled = true;
                btnSecondFloorDown.IsEnabled = true;
                btnSecondFloorUp.IsEnabled = true;
                btnThirdFloorDown.IsEnabled = true;
                btnThirdFloorUp.IsEnabled = true;
                btnFourthFloorDown.IsEnabled = true;
                StartButton.IsEnabled = false;
                AutoMode = false;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            sliderManualAuto.IsEnabled = false;
            listBoxRequestPool.Items.Add("Auto processing 30 passengers.");
            Thread myThread = new Thread(() => AddAutoPassenger());
            myThread.Start();
        }

        private void btnOpenLogFile_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", "passengerLog.txt");
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int track = rnd.Next(1, 4);
            if (track == 1)
            {
                mplayer.Open(new Uri("Elevator_Music_1.mp3", UriKind.Relative));
                mplayer.Play();
            }
            else if (track == 2)
            {
                mplayer.Open(new Uri("Elevator_Music_2.mp3", UriKind.Relative));
                mplayer.Play();
            }
            else
            {
                mplayer.Open(new Uri("Elevator_Music_3.mp3", UriKind.Relative));
                mplayer.Play();
            }
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            if(playPause == false)
            {
                mplayer.Pause();
                playPause = true;
            }
            else
            {
                mplayer.Play();
                playPause = false;
            }
            btnOpenLogFile.Focus();
            
        }

        #endregion


    }
}
