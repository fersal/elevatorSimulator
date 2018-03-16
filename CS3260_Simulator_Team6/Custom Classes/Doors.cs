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
    public class Doors
    {
        private bool isDoorsOpen;
        private bool doorsInOperation;
        private Storyboard closeDoorOperation;
        private Storyboard openDoorOperation;
        private Dictionary<int,Rectangle> leftDoors;
        private Dictionary<int, Rectangle> rightDoors;
        private const int CLOSED_DOOR_WIDTH = 47;
        private const int OPEN_DOOR_WIDTH = 2;
        private MainWindow window;
        private int currentFloor;
        private Direction elevatorDirection;
        private string direction;


        public Doors()
        {
            elevatorDirection = Direction.None;
            isDoorsOpen = false;
            doorsInOperation = false;
            window = (MainWindow)Application.Current.MainWindow;
            leftDoors = new Dictionary<int, Rectangle>();
            rightDoors = new Dictionary<int, Rectangle>();
            this.leftDoors.Add(3,window.fourthFloorDoorLeft);
            this.leftDoors.Add(2, window.thirdFloorDoorLeft);
            this.leftDoors.Add(1, window.secondFloorDoorLeft);
            this.leftDoors.Add(0, window.firstFloorDoorLeft);
            this.rightDoors.Add(3, window.fourthFloorDoorRight);
            this.rightDoors.Add(2, window.thirdFloorDoorRight);
            this.rightDoors.Add(1, window.secondFloorDoorRight);
            this.rightDoors.Add(0, window.firstFloorDoorRight);
        }

        public void CloseDoors(int currentFloor)
        {
            doorsInOperation = true;

            App.Current.Dispatcher.Invoke((Action)delegate {
                DoubleAnimation closeLeftDoorAnimation = new DoubleAnimation();
                closeLeftDoorAnimation.From = leftDoors[currentFloor].ActualWidth;
                closeLeftDoorAnimation.To = CLOSED_DOOR_WIDTH;
                closeLeftDoorAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
                DoubleAnimation closeRightDoorAnimation = new DoubleAnimation();
                closeRightDoorAnimation.From = rightDoors[currentFloor].ActualWidth;
                closeRightDoorAnimation.To = CLOSED_DOOR_WIDTH;
                closeRightDoorAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
                Storyboard.SetTargetName(closeLeftDoorAnimation, leftDoors[currentFloor].Name);
                Storyboard.SetTargetProperty(closeLeftDoorAnimation, new PropertyPath(Rectangle.WidthProperty));
                closeDoorOperation = new Storyboard();
                closeDoorOperation.Children.Add(closeLeftDoorAnimation);
                Storyboard.SetTargetName(closeRightDoorAnimation, rightDoors[currentFloor].Name);
                Storyboard.SetTargetProperty(closeRightDoorAnimation, new PropertyPath(Rectangle.WidthProperty));
                closeDoorOperation.Children.Add(closeRightDoorAnimation);
                closeDoorOperation.Begin(leftDoors[currentFloor]);
                closeDoorOperation.Begin(rightDoors[currentFloor]);
            });
        }

        public void OpenDoors(int currentFloor, Direction elevatorDirection)
        {
            doorsInOperation = true;
            this.elevatorDirection = elevatorDirection;
            if (elevatorDirection == Direction.Down)
            {
                ElevatorDirection = "Down";
            }
            else if (elevatorDirection == Direction.Up)
            {
                ElevatorDirection = "Up";
            }
            else
            {
                ElevatorDirection = "None";
            }
            CurrentFloor = currentFloor;
            App.Current.Dispatcher.Invoke((Action)delegate {
                DoubleAnimation openLeftDoorAnimation = new DoubleAnimation();
                openLeftDoorAnimation.From = leftDoors[currentFloor].ActualWidth;
                openLeftDoorAnimation.To = OPEN_DOOR_WIDTH;
                openLeftDoorAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
                DoubleAnimation openRightDoorAnimation = new DoubleAnimation();
                openRightDoorAnimation.From = rightDoors[currentFloor].ActualWidth;
                openRightDoorAnimation.To = OPEN_DOOR_WIDTH;
                openRightDoorAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
                Storyboard.SetTargetName(openLeftDoorAnimation, leftDoors[currentFloor].Name);
                Storyboard.SetTargetProperty(openLeftDoorAnimation, new PropertyPath(Rectangle.WidthProperty));
                openDoorOperation = new Storyboard();
                openDoorOperation.Children.Add(openLeftDoorAnimation);
                Storyboard.SetTargetName(openRightDoorAnimation, rightDoors[currentFloor].Name);
                Storyboard.SetTargetProperty(openRightDoorAnimation, new PropertyPath(Rectangle.WidthProperty));
                openDoorOperation.Children.Add(openRightDoorAnimation);
                openDoorOperation.Completed += (s, eA) => OndoorsHaveOpened(eA);
                openDoorOperation.Begin(leftDoors[currentFloor]);
                openDoorOperation.Begin(rightDoors[currentFloor]);
            });
        }

        public bool GetIsDoorsOpen()
        {
            return isDoorsOpen;
        }

        public bool GetIsDoorsInOperation()
        {
            return doorsInOperation;
        }

        public event EventHandler DoorsHaveOpened;
        public void OndoorsHaveOpened(EventArgs e)
        {
            EventHandler doorsHaveOpened = DoorsHaveOpened;
            if (doorsHaveOpened != null)
            {
                doorsHaveOpened(this, e);
            }
        }

        public int CurrentFloor { get { return currentFloor; } set { currentFloor = value; } }

        public string ElevatorDirection {
            get { return direction; }
            set { direction = value; }
        }
    }
}
