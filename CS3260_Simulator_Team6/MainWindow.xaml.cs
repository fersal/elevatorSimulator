using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private const int CLOSED_DOOR_WIDTH = 35;
        private const int OPEN_DOOR_WIDTH = 2;

		private Elevator elevator;
		private long travelTime;
		private Passenger passengerOne;
        private AControlsSystem command = null;
        private RequestPool request = new RequestPool();
        private IReciever click = null;
        private FloorFirstUpCommand firstUpCmd = null;
        private FloorFourDownCommand fourDownCmd = null;
        private FloorThirdUpCommand thirdUpCmd = null;
        private FloorThirdDownCommand thirdDownCmd = null;
        private FloorSecondUpCommand secondUpCmd = null;
        private FloorSecondDownCommand secondDownCmd = null;
        private InternalCloseDoorCommand closeDoorCmd = null;
        private InternalOpenDoorCommand opendDoorCmd = null;
        private InternalFirstFloorCommand firstCmd = null;
        private InternalSecondFloorCommand secondCmd = null;
        private InternalThirdFloorCommand thirdCmd = null;
        private InternalFourthFloorCommand fourthCmd = null;
        private InternalEmergencyCommand emergencyCmd = null;
        private BrushConverter bc = new BrushConverter();
        private Doors doorCmd = null;

        public MainWindow()
		{
			InitializeComponent();
            doorCmd = new Doors(request, listBoxRequestPool);
            click = new ButtonClick(request, doorCmd);
            firstUpCmd = new FloorFirstUpCommand(click);
            fourDownCmd = new FloorFourDownCommand(click);
            fourDownCmd = new FloorFourDownCommand(click);
            thirdUpCmd = new FloorThirdUpCommand(click);
            thirdDownCmd = new FloorThirdDownCommand(click);
            secondUpCmd = new FloorSecondUpCommand(click);
            secondDownCmd = new FloorSecondDownCommand(click);
            closeDoorCmd = new InternalCloseDoorCommand(click);
            opendDoorCmd = new InternalOpenDoorCommand(click);
            firstCmd = new InternalFirstFloorCommand(click);
            secondCmd = new InternalSecondFloorCommand(click);
            thirdCmd = new InternalThirdFloorCommand(click);
            fourthCmd = new InternalFourthFloorCommand(click);
            emergencyCmd = new InternalEmergencyCommand(click);
        }


		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			//elevator starts on bottom floor
			//elevator waits for a Passenger to call from another floor
			//elevator gets to floor, opens door
			//passanger enters elevator, selects destination
			//elevator closes door and travels to destination floor

			var passangerDetailWindow = new PassangerDetailWindow();
			passangerDetailWindow.DetailText.Text = PLEASE_WAIT;
			passangerDetailWindow.Show();

			passengerOne = CreatePassengerFromInputs();
			ThreadStart passangerUsesElevator = new ThreadStart(PassengerUsesElevator);
			Thread passangerThread = new Thread(passangerUsesElevator);
			passangerThread.Start();

			while (passangerThread.IsAlive)
			{
				//Empty loop just to hold Details window until Passanger thread comes back with results.
			}

			passangerDetailWindow.DetailText.Text = "Took " + travelTime + " seconds to travel from floor "
				+ passengerOne.EntryFloor + " to floor " + passengerOne.DestinationFloor;
		}

		private void PassengerUsesElevator()
		{
			elevator = new Elevator();
			travelTime = passengerOne.UseElevator(elevator);
		}

		private Passenger CreatePassengerFromInputs()
		{
			ComboBoxItem passangerTypeComboItem = (ComboBoxItem)PassangerTypeComboBox.SelectedItem;
			string passangerType = passangerTypeComboItem.Content.ToString();

			ComboBoxItem destinationItemSelected = (ComboBoxItem)DestinationFloorComboBox.SelectedItem;
			int passangerDestination = Convert.ToInt16(destinationItemSelected.Content.ToString());

			ComboBoxItem entryItemSelected = (ComboBoxItem)EntryFloorComboBox.SelectedItem;
			int passangerEntry = Convert.ToInt16(entryItemSelected.Content.ToString());

			return new Passenger(passangerEntry, passangerDestination, passangerType);
		}

        private void btnFourthFloorDown_Click(object sender, RoutedEventArgs e)
        {
            command = fourDownCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnFourthFloorDown.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnThirdFloorUp_Click(object sender, RoutedEventArgs e)
        {
            command = thirdUpCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnThirdFloorUp.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnThirdFloorDown_Click(object sender, RoutedEventArgs e)
        {
            command = thirdDownCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnThirdFloorDown.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnSecondFloorUp_Click(object sender, RoutedEventArgs e)
        {
            command = secondUpCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnSecondFloorUp.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnSecondFloorDown_Click(object sender, RoutedEventArgs e)
        {
            command = secondDownCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnSecondFloorDown.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnFirstFloorUp_Click(object sender, RoutedEventArgs e)
        {
            command = firstUpCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnFirstFloorUp.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnOpenDoor_Click(object sender, RoutedEventArgs e)
        {
            command = opendDoorCmd;
            bool clicked = command.Execute();
            if (!doorCmd.GetIsDoorsOpen())
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnOpenDoor.BorderBrush = Brushes.LightSeaGreen;
                //Only for test case
                doorCmd.OpenCloseDoors(btnOpenDoor, fourthFloorDoorLeft, fourthFloorDoorRight, "fourthFloorDoorLeft", "fourthFloorDoorRight", OPEN_DOOR_WIDTH);
            }
        StartButton.Focus();
        }

        private void btnCloseDoor_Click(object sender, RoutedEventArgs e)
        {
            command = closeDoorCmd;
            bool clicked = command.Execute();
            if (doorCmd.GetIsDoorsOpen())
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnCloseDoor.BorderBrush = Brushes.LightSeaGreen;
                //Only for test case
                doorCmd.OpenCloseDoors(btnCloseDoor, fourthFloorDoorLeft, fourthFloorDoorRight, "fourthFloorDoorLeft", "fourthFloorDoorRight", CLOSED_DOOR_WIDTH);
            }
            StartButton.Focus();
        }

        private void btnFourthFloor_Click(object sender, RoutedEventArgs e)
        {
            command = fourthCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnFourthFloor.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnThirdFloor_Click(object sender, RoutedEventArgs e)
        {
            command = thirdCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnThirdFloor.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnSecondFloor_Click(object sender, RoutedEventArgs e)
        {
            command = secondCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnSecondFloor.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void btnFirstFloor_Click(object sender, RoutedEventArgs e)
        {
            command = firstCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                btnFirstFloor.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
        }

        private void Emergency_Click(object sender, RoutedEventArgs e)
        {
            command = emergencyCmd;
            bool clicked = command.Execute();
            if (!clicked)
            {
                listBoxRequestPool.Items.Add(request.GetLastRequest());
                Emergency.BorderBrush = Brushes.LightSeaGreen;
            }
            StartButton.Focus();
            //else
            //{
            //    Emergency.BorderBrush = (Brush)bc.ConvertFrom("#FF990B0B");
            //}
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
        bool GetResult();
    }
    public abstract class AControlsSystem
    {
        protected IReciever reciever_ = null;
        public AControlsSystem(IReciever reciever)
        {
            this.reciever_ = reciever;
        }

        public abstract bool Execute();
    }

    public class FloorFourDownCommand : AControlsSystem
    {
        public FloorFourDownCommand(IReciever reciever) : base(reciever) { }

        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorFourDown);
            return reciever_.GetResult();
        }
    }

    public class FloorThirdUpCommand : AControlsSystem
    {
        public FloorThirdUpCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeUp);
            return reciever_.GetResult();
        }
    }

    public class FloorThirdDownCommand : AControlsSystem
    {
        public FloorThirdDownCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorThreeDown);
            return reciever_.GetResult();
        }
    }

    public class FloorSecondUpCommand : AControlsSystem
    {
        public FloorSecondUpCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoUp);
            return reciever_.GetResult();
        }
    }

    public class FloorSecondDownCommand : AControlsSystem
    {
        public FloorSecondDownCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoDown);
            return reciever_.GetResult();
        }
    }

    public class FloorFirstUpCommand : AControlsSystem
    {
        public FloorFirstUpCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorOneUp);
            return reciever_.GetResult();
        }
    }

    public class InternalCloseDoorCommand : AControlsSystem
    {
        public InternalCloseDoorCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.intCloseDoor);
            return reciever_.GetResult();
        }
    }

    public class InternalOpenDoorCommand : AControlsSystem
    {
        public InternalOpenDoorCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.intOpenDoor);
            return reciever_.GetResult();
        }
    }

    public class InternalFourthFloorCommand : AControlsSystem
    {
        public InternalFourthFloorCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.intFour);
            return reciever_.GetResult();
        }
    }

    public class InternalThirdFloorCommand : AControlsSystem
    {
        public InternalThirdFloorCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.intThree);
            return reciever_.GetResult();
        }
    }

    public class InternalSecondFloorCommand : AControlsSystem
    {
        public InternalSecondFloorCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.intTwo);
            return reciever_.GetResult();
        }
    }

    public class InternalFirstFloorCommand : AControlsSystem
    {
        public InternalFirstFloorCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.intOne);
            return reciever_.GetResult();
        }
    }

    public class InternalEmergencyCommand : AControlsSystem
    {
        public InternalEmergencyCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.intEmerg);
            return reciever_.GetResult();
        }
    }

    public class ButtonClick : IReciever
    {
        RequestPool request_;
        Doors doors_;
        bool floorOneUp_, floorTwoUp_, floorTwoDown_, floorThreeUp_,
            floorThreeDown_, floorFourDown_,
            intFloorFour_, intFloorThree_, intFloorTwo_, intFloorOne_,
            intEmerg_;
        ACTION_LIST currentAction;

        public ButtonClick(RequestPool request, Doors doors)
        {
            floorOneUp_ = false; floorTwoUp_ = false; floorTwoDown_ = false; floorThreeUp_ = false;
            floorThreeDown_ = false; floorFourDown_ = false;
            intFloorFour_ = false; intFloorThree_ = false; intFloorTwo_ = false; intFloorOne_ = false;
            intEmerg_ = false;
            request_ = request;
            doors_ = doors;
        }

        public bool GetResult()
        {
            bool result = false;

            if (currentAction == ACTION_LIST.floorFourDown)
            {
                if (floorFourDown_)
                {
                    result = true;
                    floorFourDown_ = false;
                }
                else
                {
                    floorFourDown_ = true;
                    request_.AddRequest("Floor 4 Down Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeUp)
            {
                if (floorThreeUp_)
                {
                    result = true;
                    floorThreeUp_ = false;
                }
                else
                {
                    floorThreeUp_ = true;
                    request_.AddRequest("Floor 3 Up Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeDown)
            {
                if (floorThreeDown_)
                {
                    result = true;
                    floorThreeDown_ = false;
                }
                else
                {
                    floorThreeDown_ = true;
                    request_.AddRequest("Floor 3 Down Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoUp)
            {
                if (floorTwoUp_)
                {
                    result = true;
                    floorTwoUp_ = false;
                }
                else
                {
                    floorTwoUp_ = true;
                    request_.AddRequest("Floor 2 Up Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoDown)
            {
                if (floorTwoDown_)
                {
                    result = true;
                    floorTwoDown_ = false;
                }
                else
                {
                    floorTwoDown_ = true;
                    request_.AddRequest("Floor 2 Down Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.floorOneUp)
            {
                if (floorOneUp_)
                {
                    result = true;
                    floorOneUp_ = false;
                }
                else
                {
                    floorOneUp_ = true;
                    request_.AddRequest("Floor 1 Up Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.intCloseDoor)
            {
                if (doors_.GetIsDoorsOpen())
                {
                    request_.AddRequest("Close Elevator Doors Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intOpenDoor)
            {
                if (!doors_.GetIsDoorsOpen())
                {
                    request_.AddRequest("Open Elevator Doors Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.intEmerg)
            {
                if (intEmerg_)
                {
                    result = true;
                    intEmerg_ = false;
                }
                else
                {
                    intEmerg_ = true;
                    request_.AddRequest("Halt Elevator Emergency Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.intFour)
            {
                if (intFloorFour_)
                {
                    result = true;
                    intFloorFour_ = false;
                }
                else
                {
                    intFloorFour_ = true;
                    request_.AddRequest("Travel to 4th floor Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.intThree)
            {
                if (intFloorThree_)
                {
                    result = true;
                    intFloorThree_ = false;
                }
                else
                {
                    intFloorThree_ = true;
                    request_.AddRequest("Travel to 3rd floor Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.intTwo)
            {
                if (intFloorTwo_)
                {
                    result = true;
                    intFloorTwo_ = false;
                }
                else
                {
                    intFloorTwo_ = true;
                    request_.AddRequest("Travel to 2nd floor Request");
                    result = false;
                }
            }
            else if (currentAction == ACTION_LIST.intOne)
            {
                if (intFloorOne_)
                {
                    result = true;
                    intFloorOne_ = false;
                }
                else
                {
                    intFloorOne_ = true;
                    request_.AddRequest("Travel to 1st floor Request");
                    result = false;
                }
            }
            return result;
        }

        public void SetAction(ACTION_LIST action)
        {
            currentAction = action;
        }
    }

    public class RequestPool
    {
        private List<string> requestPool;
        private string currentRequest;

        public RequestPool()
        {
            requestPool = new List<string>();
            currentRequest = "";
        }

        public void AddRequest(string request)
        {
            requestPool.Add(request);
            currentRequest = request;
        }

        public void CompleteRequest(string request)
        {
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

    public class Doors
    {
        RequestPool request_;
        private bool isDoorsOpen;
        private bool doorsInOperation;
        private ListBox list_;
        private Storyboard doorOperation;
        private string lastOperation;

        public Doors(RequestPool request, ListBox list)
        {
            isDoorsOpen = false;
            doorsInOperation = false;
            request_ = request;
            list_ = list;
        }

        public void OpenCloseDoors(Button button, Rectangle leftDoor, Rectangle rightDoor, string leftDoorName, string rightDoorName, int toSize)
        {
            if(doorsInOperation == true)
            {
                doorOperation.Stop();
                string removelistitem = lastOperation;
                for (int i = 0; i < list_.Items.Count; i++)
                {
                    if (list_.Items[i].ToString().Contains(removelistitem))
                    {
                        list_.Items.RemoveAt(i);
                        request_.CompleteRequest(lastOperation);
                        button.BorderBrush = null;
                        if (isDoorsOpen)
                        {
                            isDoorsOpen = false;
                        }
                        else
                        {
                            isDoorsOpen = true;
                        }
                        doorsInOperation = false;
                    }
                }
            }
            doorsInOperation = true;
            lastOperation = request_.GetCurrentRequest();
            DoubleAnimation leftDoorAnimation = new DoubleAnimation();
            leftDoorAnimation.From = leftDoor.ActualWidth;
            leftDoorAnimation.To = toSize;
            leftDoorAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
            DoubleAnimation rightDoorAnimation = new DoubleAnimation();
            rightDoorAnimation.From = rightDoor.ActualWidth;
            rightDoorAnimation.To = toSize;
            rightDoorAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
            Storyboard.SetTargetName(leftDoorAnimation, leftDoorName);
            Storyboard.SetTargetProperty(leftDoorAnimation, new PropertyPath(Rectangle.WidthProperty));
            doorOperation = new Storyboard();
            doorOperation.Children.Add(leftDoorAnimation);
            Storyboard.SetTargetName(rightDoorAnimation, rightDoorName);
            Storyboard.SetTargetProperty(rightDoorAnimation, new PropertyPath(Rectangle.WidthProperty));
            doorOperation.Children.Add(rightDoorAnimation);
            doorOperation.Completed += (s, eA) => doorAni_Complete(doorOperation, button);
            doorOperation.Begin(leftDoor);
            doorOperation.Begin(rightDoor);
        }

        public bool GetIsDoorsOpen()
        {
            return isDoorsOpen;
        }

        public bool GetIsDoorsInOperation()
        {
            return doorsInOperation;
        }

        private void doorAni_Complete(Storyboard sb, Button button)
        {
            string request = button.Name;
            string removelistitem;
            if (request == "btnCloseDoor")
            {
                removelistitem = "Close Elevator Doors Request";
            }
            else if(request == "btnOpenDoor")
            {
                removelistitem = "Open Elevator Doors Request";
            }
            else
            {
                removelistitem = request_.GetCurrentRequest();
            }
            for (int i = 0; i < list_.Items.Count; i++)
            {
                if (list_.Items[i].ToString().Contains(removelistitem))
                {
                    list_.Items.RemoveAt(i);
                    request_.CompleteRequest(removelistitem);
                    button.BorderBrush = null;
                    if (isDoorsOpen)
                    {
                        isDoorsOpen = false;
                    }
                    else
                    {
                        isDoorsOpen = true;
                    }
                    doorsInOperation = false;
                }
            }
        }
    }
}
