using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ADULT = "Adult";
        private const string CHILD = "Child";
        private const string PLEASE_WAIT = "It could be up to 40 seconds to get to the 8th floor\n		PLEASE WAIT....";
        private int prevFloorFourSelectedImg = 0;
        private int fourthFloorPassengerCount = 0;
        private int prevFloorThreeSelectedImg = 0;
        private int thirdFloorPassengerCount = 0;
        private int thirdFloorUpPassengerCount = 0;
        private int thirdFloorDownPassengerCount = 0;
        private int prevFloorTwoSelectedImg = 0;
        private int secondFloorPassengerCount = 0;
        private int secondFloorUpPassengerCount = 0;
        private int secondFloorDownPassengerCount = 0;
        private int prevFloorOneSelectedImg = 0;
        private int firstFloorPassengerCount = 0;
        private const int MAX_PASSANGERS = 8;
        private int totalPassengerCountID = 0;
        private bool fourthFloorDownButtonClicked;
        private bool thirdFloorDownButtonClicked;
        private bool secondFloorDownButtonClicked;
        private bool thirdFloorUpButtonClicked;
        private bool secondFloorUpButtonClicked;
        private bool firstFloorUpButtonClicked;
        public Building MyBuilding;
        private AddNewPassenger AddPassenger = null;
        private List<Image> fourthFloorPassengerImages = new List<Image>();
        private List<int> fourthFloorPassengerDestination = new List<int>();
        private List<Image> thirdFloorUpPassengerImages = new List<Image>();
        private List<int> thirdFloorUpPassengerDestination = new List<int>();
        private List<Image> secondFloorUpPassengerImages = new List<Image>();
        private List<int> secondFloorUpPassengerDestination = new List<int>();
        private List<Image> thirdFloorDownPassengerImages = new List<Image>();
        private List<int> thirdFloorDownPassengerDestination = new List<int>();
        private List<Image> secondFloorDownPassengerImages = new List<Image>();
        private List<int> secondFloorDownPassengerDestination = new List<int>();
        private List<Image> firstFloorPassengerImages = new List<Image>();
        private List<int> firstFloorPassengerDestination = new List<int>();

        //      private Elevator elevator;
        //private long travelTime;
        //private Passenger passengerOne;
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
        FloorSelectionDialog dialog = null;

        public MainWindow()
        {
            InitializeComponent();
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
            AddPassenger = new AddNewPassenger();
            this.doors.DoorsHaveOpened += new EventHandler(this.Doors_DoorsHaveOpened);

        }

        public void Doors_DoorsHaveOpened(object sender, EventArgs e)
        {
            int floor = doors.CurrentFloor;
            bool clicked = false;
            string direction = doors.ElevatorDirection;
            if (floor == 3)
            {
                command = fourDownCmd;
                clicked = command.CheckClick();
                if (clicked)
                {
                    command.UnExecute();
                    btnFourthFloorDown.BorderBrush = null;
                    fourthFloorPassengerCount = 0;
                }
            }
            else if (floor == 2)
            {
                command = thirdDownCmd;
                clicked = command.CheckClick();
                if (clicked)
                {
                    command.UnExecute();
                    btnThirdFloorDown.BorderBrush = null;
                    thirdFloorDownPassengerCount = 0;
                }

                command = thirdUpCmd;
                clicked = command.CheckClick();
                if (clicked)
                {
                    command.UnExecute();
                    btnThirdFloorUp.BorderBrush = null;
                    thirdFloorUpPassengerCount = 0;
                }

                thirdFloorPassengerCount = 0;
            }
            else if (floor == 1)
            {

                command = secondDownCmd;
                clicked = command.CheckClick();
                if (clicked)
                {
                    command.UnExecute();
                    btnSecondFloorDown.BorderBrush = null;
                    secondFloorDownPassengerCount = 0;
                }

                command = secondUpCmd;
                clicked = command.CheckClick();
                if (clicked)
                {
                    command.UnExecute();
                    btnSecondFloorUp.BorderBrush = null;
                    secondFloorUpPassengerCount = 0;
                }

                secondFloorPassengerCount = 0;
            }
            else if (floor == 0)
            {
                command = firstUpCmd;
                clicked = command.CheckClick();
                if (clicked)
                {
                    command.UnExecute();
                    btnFirstFloorUp.BorderBrush = null;
                    firstFloorPassengerCount = 0;
                }
            }
        }


        private void btnFourthFloorDown_Click(object sender, RoutedEventArgs e)
        {
            if (fourthFloorPassengerCount != 0)
            {
                command = fourDownCmd;
                fourthFloorDownButtonClicked = command.Execute();
                if (!fourthFloorDownButtonClicked)
                {
                    btnFourthFloorDown.BorderBrush = Brushes.LightSeaGreen;
                    AddPassenger.FloorIndex = 3;
                    for (int i = 0; i < fourthFloorPassengerCount; i++)
                    {
                        AddPassenger.AddPassenger(fourthFloorPassengerDestination[i], fourthFloorPassengerImages[i]);
                    }
                    fourthFloorPassengerImages.Clear();
                    AddPassenger.LoadPassenger();
                }
            }
            else
            {
                MessageBox.Show("Please add passengers first.");
            }
            StartButton.Focus();
        }

        private void btnThirdFloorUp_Click(object sender, RoutedEventArgs e)
        {
            if (thirdFloorUpPassengerCount != 0)
            {
                command = thirdUpCmd;
                thirdFloorUpButtonClicked = command.Execute();
                if (!thirdFloorUpButtonClicked)
                {
                    btnThirdFloorUp.BorderBrush = Brushes.LightSeaGreen;
                    AddPassenger.FloorIndex = 2;
                    for (int i = 0; i < thirdFloorUpPassengerCount; i++)
                    {
                        if (thirdFloorUpPassengerDestination[i] > 2)
                            AddPassenger.AddPassenger(thirdFloorUpPassengerDestination[i], thirdFloorUpPassengerImages[i]);
                    }
                    thirdFloorUpPassengerImages.Clear();
                    AddPassenger.LoadPassenger();
                }
            }
            else
            {
                MessageBox.Show("Please add passengers to go up first.");
            }
            StartButton.Focus();
        }

        private void btnThirdFloorDown_Click(object sender, RoutedEventArgs e)
        {
            if (thirdFloorDownPassengerCount != 0)
            {
                command = thirdDownCmd;
                thirdFloorDownButtonClicked = command.Execute();
                if (!thirdFloorDownButtonClicked)
                {
                    btnThirdFloorDown.BorderBrush = Brushes.LightSeaGreen;
                    AddPassenger.FloorIndex = 2;
                    for (int i = 0; i < thirdFloorDownPassengerCount; i++)
                    {
                        if (thirdFloorDownPassengerDestination[i] < 2)
                            AddPassenger.AddPassenger(thirdFloorDownPassengerDestination[i], thirdFloorDownPassengerImages[i]);
                    }
                    thirdFloorDownPassengerImages.Clear();
                    AddPassenger.LoadPassenger();
                }
            }
            else
            {
                MessageBox.Show("Please add passengers to go down first.");
            }
            StartButton.Focus();
        }

        private void btnSecondFloorUp_Click(object sender, RoutedEventArgs e)
        {
            if (secondFloorUpPassengerCount != 0)
            {
                command = secondUpCmd;
                secondFloorUpButtonClicked = command.Execute();
                if (!secondFloorUpButtonClicked)
                {
                    btnSecondFloorUp.BorderBrush = Brushes.LightSeaGreen;
                    AddPassenger.FloorIndex = 1;
                    for (int i = 0; i < secondFloorUpPassengerCount; i++)
                    {
                        if (secondFloorUpPassengerDestination[i] > 1)
                            AddPassenger.AddPassenger(secondFloorUpPassengerDestination[i], secondFloorUpPassengerImages[i]);
                    }
                    secondFloorUpPassengerImages.Clear();
                    AddPassenger.LoadPassenger();
                }
            }
            else
            {
                MessageBox.Show("Please add passengers to go up first.");
            }
            StartButton.Focus();
        }

        private void btnSecondFloorDown_Click(object sender, RoutedEventArgs e)
        {
            if (secondFloorDownPassengerCount != 0)
            {
                command = secondDownCmd;
                secondFloorDownButtonClicked = command.Execute();
                if (!secondFloorDownButtonClicked)
                {
                    btnSecondFloorDown.BorderBrush = Brushes.LightSeaGreen;
                    AddPassenger.FloorIndex = 1;
                    for (int i = 0; i < secondFloorDownPassengerCount; i++)
                    {
                        if (secondFloorDownPassengerDestination[i] < 1)
                            AddPassenger.AddPassenger(secondFloorDownPassengerDestination[i], secondFloorDownPassengerImages[i]);
                    }
                    secondFloorDownPassengerImages.Clear();
                    AddPassenger.LoadPassenger();
                }
            }
            else
            {
                MessageBox.Show("Please add passengers to go down first.");
            }
            StartButton.Focus();
        }

        private void btnFirstFloorUp_Click(object sender, RoutedEventArgs e)
        {
            if (firstFloorPassengerCount != 0)
            {
                command = firstUpCmd;
                firstFloorUpButtonClicked = command.Execute();
                if (!firstFloorUpButtonClicked)
                {
                    btnFirstFloorUp.BorderBrush = Brushes.LightSeaGreen;
                    AddPassenger.FloorIndex = 0;
                    for (int i = 0; i < firstFloorPassengerCount; i++)
                    {
                        AddPassenger.AddPassenger(firstFloorPassengerDestination[i], firstFloorPassengerImages[i]);
                    }
                    firstFloorPassengerImages.Clear();
                    AddPassenger.LoadPassenger();
                }
            }
            else
            {
                MessageBox.Show("Please add passengers first.");
            }
            StartButton.Focus();
        }

        private Image FloorFourAddPeople()
        {
            int peopleSelected = fourthFloorPassengerCount;
            Image selectedImg = null;
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
            }
            return selectedImg;
        }

        private Image FloorThreeAddPeople()
        {
            int peopleSelected = thirdFloorPassengerCount;
            Image selectedImg = null;
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
            }
            return selectedImg;
        }

        private Image FloorTwoAddPeople()
        {
            int peopleSelected = secondFloorPassengerCount;
            Image selectedImg = null;
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
            }
            return selectedImg;
        }

        private Image FloorOneAddPeople()
        {
            int peopleSelected = firstFloorPassengerCount;
            Image selectedImg = null;
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
            }
            return selectedImg;
        }

        private void btnAddPassengerFloorFour_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 4;
            fourthFloorPassengerCount++;
            if (fourthFloorPassengerCount <= MAX_PASSANGERS)
            {
                fourthFloorPassengerImages.Add(FloorFourAddPeople());
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
                totalPassengerCountID++;
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
                fourthFloorPassengerCount--;
            }
        }

        private void btnAddPassengerFloorThree_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 3;
            thirdFloorPassengerCount++;
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
                }
                else if (selectedFloor < 2)
                {
                    thirdFloorDownPassengerDestination.Add(selectedFloor);
                    thirdFloorDownPassengerImages.Add(selectedImage);
                    thirdFloorDownPassengerCount++;
                }

                totalPassengerCountID++;
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
                thirdFloorPassengerCount--;
            }

        }

        private void btnAddPassengerFloorTwo_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 2;
            secondFloorPassengerCount++;
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
                }
                else if (selectedFloor < 1)
                {
                    secondFloorDownPassengerDestination.Add(selectedFloor);
                    secondFloorDownPassengerImages.Add(selectedImage);
                    secondFloorDownPassengerCount++;
                }

                totalPassengerCountID++;
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
                secondFloorPassengerCount--;
            }
        }

        private void btnAddPassengerFloorOne_Click(object sender, RoutedEventArgs e)
        {
            int floorIndex = 1;
            firstFloorPassengerCount++;
            if (firstFloorPassengerCount <= MAX_PASSANGERS)
            {
                firstFloorPassengerImages.Add(FloorOneAddPeople());
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
            }
            else
            {
                MessageBox.Show("The max amount of passengers have been added for this floor.");
                firstFloorPassengerCount--;
            }
        }
    }

    public class AddNewPassenger
    {
        private int floorIndex;
        private Passenger NewPassenger;
        MainWindow window = (MainWindow)Application.Current.MainWindow;


        public int FloorIndex { get { return floorIndex; } set { floorIndex = value; } }

        public MainWindow MyForm { get { return window; } private set { } }

        public Floor MyFloor
        {
            get { return (MyForm.MyBuilding.ArrayOfAllFloors[this.floorIndex]); }
            private set { }
        }

        public void AddPassenger(int destination, Image personImage)
        {
            if (MyFloor.GetCurrentAmmountOfPeopleInTheQueue() >= MyFloor.GetMaximumAmmountOfPeopleInTheQueue())
            {
                MessageBox.Show("It looks like the corridor is too crowdy now. Please, wait a while until elevators take few passengers away.", "Your passenger has to wait");
                return;
            }

            NewPassenger = new Passenger(MyForm.MyBuilding, this.MyFloor, destination, personImage);
        }


        public void LoadPassenger()
        {
            this.MyFloor.OnNewPassengerAppeared(new PassengerEventArgs(NewPassenger));
        }
    }

    public enum ACTION_LIST
    {
        floorFourDown,
        floorThreeUp, floorThreeDown,
        floorTwoUp, floorTwoDown,
        floorOneUp, intOpenDoor,
        intCloseDoor, intFour,
        intThree, intTwo,
        intOne, intEmerg
    }

    public interface IReciever
    {
        void SetAction(ACTION_LIST action);
        bool CLick();
        void UnClick();
        bool isClicked();
    }
    public abstract class AControlsSystem
    {
        protected IReciever reciever_ = null;
        public AControlsSystem(IReciever reciever)
        {
            this.reciever_ = reciever;
        }

        public abstract bool Execute();

        public abstract void UnExecute();

        public abstract bool CheckClick();
    }

    public class FloorFourDownCommand : AControlsSystem
    {
        public FloorFourDownCommand(IReciever reciever) : base(reciever) { }

        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorFourDown);
            return reciever_.CLick();
        }

        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorFourDown);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorFourDown);
            return reciever_.isClicked();
        }
    }

    public class FloorThirdUpCommand : AControlsSystem
    {
        public FloorThirdUpCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeUp);
            return reciever_.CLick();
        }
        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeUp);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeUp);
            return reciever_.isClicked();
        }
    }

    public class FloorThirdDownCommand : AControlsSystem
    {
        public FloorThirdDownCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeDown);
            return reciever_.CLick();
        }
        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeDown);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeDown);
            return reciever_.isClicked();
        }
    }

    public class FloorSecondUpCommand : AControlsSystem
    {
        public FloorSecondUpCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoUp);
            return reciever_.CLick();
        }
        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoUp);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoUp);
            return reciever_.isClicked();
        }
    }

    public class FloorSecondDownCommand : AControlsSystem
    {
        public FloorSecondDownCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoDown);
            return reciever_.CLick();
        }
        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoDown);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoDown);
            return reciever_.isClicked();
        }
    }

    public class FloorFirstUpCommand : AControlsSystem
    {
        public FloorFirstUpCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorOneUp);
            return reciever_.CLick();
        }
        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorOneUp);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorOneUp);
            return reciever_.isClicked();
        }
    }



    public class ButtonClick : IReciever
    {
        RequestPool request_;
        bool floorOneUp_, floorTwoUp_, floorTwoDown_, floorThreeUp_,
            floorThreeDown_, floorFourDown_, doorsClosed_, doorsOpen_,
            intFloorFour_, intFloorThree_, intFloorTwo_, intFloorOne_,
            intEmerg_;
        ACTION_LIST currentAction;

        public ButtonClick(RequestPool request)
        {
            floorOneUp_ = false; floorTwoUp_ = false; floorTwoDown_ = false; floorThreeUp_ = false;
            floorThreeDown_ = false; floorFourDown_ = false; doorsClosed_ = true; doorsOpen_ = false;
            intFloorFour_ = false; intFloorThree_ = false; intFloorTwo_ = false; intFloorOne_ = false;
            intEmerg_ = false;
            request_ = request;

        }

        public bool CLick()
        {
            bool result = false;

            if (currentAction == ACTION_LIST.floorFourDown)
            {
                if (!floorFourDown_)
                {
                    floorFourDown_ = true;
                    request_.AddRequest("Floor 4 Down Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeUp)
            {
                if (!floorThreeUp_)
                {
                    floorThreeUp_ = true;
                    request_.AddRequest("Floor 3 Up Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeDown)
            {
                if (!floorThreeDown_)
                {
                    floorThreeDown_ = true;
                    request_.AddRequest("Floor 3 Down Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoUp)
            {
                if (!floorTwoUp_)
                {
                    floorTwoUp_ = true;
                    request_.AddRequest("Floor 2 Up Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoDown)
            {
                if (!floorTwoDown_)
                {
                    floorTwoDown_ = true;
                    request_.AddRequest("Floor 2 Down Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorOneUp)
            {
                if (!floorOneUp_)
                {
                    floorOneUp_ = true;
                    request_.AddRequest("Floor 1 Up Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intCloseDoor)
            {
                if (!doorsClosed_)
                {
                    request_.AddRequest("Close Elevator Doors Request");
                    doorsClosed_ = true;
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intOpenDoor)
            {
                if (!doorsOpen_)
                {
                    request_.AddRequest("Open Elevator Doors Request");
                    doorsOpen_ = true;
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intEmerg)
            {
                if (!intEmerg_)
                {
                    intEmerg_ = true;
                    request_.AddRequest("Halt Elevator Emergency Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intFour)
            {
                if (!intFloorFour_)
                {
                    intFloorFour_ = true;
                    request_.AddRequest("Travel to 4th floor Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intThree)
            {
                if (intFloorThree_)
                {
                    intFloorThree_ = true;
                    request_.AddRequest("Travel to 3rd floor Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intTwo)
            {
                if (intFloorTwo_)
                {
                    intFloorTwo_ = true;
                    request_.AddRequest("Travel to 2nd floor Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intOne)
            {
                if (intFloorOne_)
                {
                    intFloorOne_ = true;
                    request_.AddRequest("Travel to 1st floor Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public void SetAction(ACTION_LIST action)
        {
            currentAction = action;
        }

        public void UnClick()
        {
            if (currentAction == ACTION_LIST.floorFourDown)
            {
                if (floorFourDown_)
                {
                    floorFourDown_ = false;
                    request_.CompleteRequest("Floor 4 Down Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeUp)
            {
                if (floorThreeUp_)
                {
                    floorThreeUp_ = false;
                    request_.CompleteRequest("Floor 3 Up Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeDown)
            {
                if (floorThreeDown_)
                {
                    floorThreeDown_ = false;
                    request_.CompleteRequest("Floor 3 Down Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoUp)
            {
                if (floorTwoUp_)
                {
                    floorTwoUp_ = false;
                    request_.CompleteRequest("Floor 2 Up Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoDown)
            {
                if (floorTwoDown_)
                {
                    floorTwoDown_ = false;
                    request_.CompleteRequest("Floor 2 Down Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorOneUp)
            {
                if (floorOneUp_)
                {
                    floorOneUp_ = false;
                    request_.CompleteRequest("Floor 1 Up Request");
                }
            }
            else if (currentAction == ACTION_LIST.intCloseDoor)
            {
                if (doorsClosed_)
                {
                    doorsClosed_ = false;
                    request_.CompleteRequest("Close Elevator Doors Request");
                }
            }
            else if (currentAction == ACTION_LIST.intOpenDoor)
            {
                if (doorsOpen_)
                {
                    doorsOpen_ = false;
                    request_.CompleteRequest("Open Elevator Doors Request");
                }
            }
            else if (currentAction == ACTION_LIST.intEmerg)
            {
                if (intEmerg_)
                {
                    intEmerg_ = false;
                    request_.CompleteRequest("Halt Elevator Emergency Request");
                }
            }
            else if (currentAction == ACTION_LIST.intFour)
            {
                if (intFloorFour_)
                {
                    intFloorFour_ = false;
                    request_.CompleteRequest("Travel to 4th floor Request");
                }
            }
            else if (currentAction == ACTION_LIST.intThree)
            {
                if (intFloorThree_)
                {
                    intFloorThree_ = false;
                    request_.CompleteRequest("Travel to 3rd floor Request");
                }
            }
            else if (currentAction == ACTION_LIST.intTwo)
            {
                if (intFloorTwo_)
                {
                    intFloorTwo_ = false;
                    request_.CompleteRequest("Travel to 2nd floor Request");
                }
            }
            else if (currentAction == ACTION_LIST.intOne)
            {
                if (intFloorOne_)
                {
                    intFloorOne_ = false;
                    request_.CompleteRequest("Travel to 1st floor Request");
                }
            }
        }

        public bool isClicked()
        {
            bool result = false;
            if (currentAction == ACTION_LIST.floorFourDown)
            {
                result = floorFourDown_;
            }
            else if (currentAction == ACTION_LIST.floorThreeUp)
            {
                result = floorThreeUp_;
            }
            else if (currentAction == ACTION_LIST.floorThreeDown)
            {
                result = floorThreeDown_;
            }
            else if (currentAction == ACTION_LIST.floorTwoUp)
            {
                result = floorTwoUp_;
            }
            else if (currentAction == ACTION_LIST.floorTwoDown)
            {
                result = floorTwoDown_;
            }
            else if (currentAction == ACTION_LIST.floorOneUp)
            {
                result = floorOneUp_;
            }
            return result;
        }
    }

    public class RequestPool
    {
        private List<string> requestPool;
        private string currentRequest;
        private ListBox lstBoxRequestPool;

        public RequestPool(ListBox listBox)
        {
            requestPool = new List<string>();
            currentRequest = "";
            this.lstBoxRequestPool = listBox;
        }

        public void AddRequest(string request)
        {
            requestPool.Add(request);
            lstBoxRequestPool.Items.Add(request);
            currentRequest = request;
        }

        public void CompleteRequest(string request)
        {
            for (int i = 0; i < lstBoxRequestPool.Items.Count; i++)
            {
                if (lstBoxRequestPool.Items[i].ToString().Contains(request))
                {
                    lstBoxRequestPool.Items.RemoveAt(i);
                }
            }
            requestPool.Remove(request);
        }

        public string GetLastRequest()
        {
            return requestPool.Last();
        }

        public string GetCurrentRequest()
        {
            return currentRequest;
        }
    }
}
