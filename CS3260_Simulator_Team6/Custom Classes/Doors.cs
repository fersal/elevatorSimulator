﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace CS3260_Simulator_Team6
{
    public class Doors
    {
        #region FIELDS

        private bool isDoorsOpen;
        private bool doorsInOperation;
        private Storyboard closeDoorOperation;
        private Storyboard openDoorOperation;
        private Dictionary<int, Rectangle> leftDoors;
        private Dictionary<int, Rectangle> rightDoors;
        private const int CLOSED_DOOR_WIDTH = 47;
        private const int OPEN_DOOR_WIDTH = 2;
        private MainWindow window;
        private int currentFloor;
        private string direction;

        #endregion FIELDS

        #region METHODS

        /// <summary>
        /// Purpose: Doors Constructor
        /// </summary>
        /// Purpose: Construct Doors class
        /// Returns: None
        /// -----------------------------------------------------------------
        public Doors()
        {
            isDoorsOpen = false;
            doorsInOperation = false;
            window = (MainWindow)System.Windows.Application.Current.MainWindow;
            leftDoors = new Dictionary<int, Rectangle>();
            rightDoors = new Dictionary<int, Rectangle>();
            this.leftDoors.Add(3, window.fourthFloorDoorLeft);
            this.leftDoors.Add(2, window.thirdFloorDoorLeft);
            this.leftDoors.Add(1, window.secondFloorDoorLeft);
            this.leftDoors.Add(0, window.firstFloorDoorLeft);
            this.rightDoors.Add(3, window.fourthFloorDoorRight);
            this.rightDoors.Add(2, window.thirdFloorDoorRight);
            this.rightDoors.Add(1, window.secondFloorDoorRight);
            this.rightDoors.Add(0, window.firstFloorDoorRight);
        }

        /// <summary>
        /// Purpose: Close the elevator doors
        /// </summary>
        /// <param name="currentFloor">Current floor index</param>
        /// Purpose: Animate doors to close
        /// Returns: None
        /// -----------------------------------------------------------------
        public void CloseDoors(int currentFloor)
        {
            doorsInOperation = true;

            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
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

        /// <summary>
        /// Purpose: Open the elevator doors
        /// </summary>
        /// <param name="currentFloor">Current floor index</param>
        /// Purpose: Animate doors to open
        /// Returns: None
        /// -----------------------------------------------------------------
        public void OpenDoors(int currentFloor)
        {
            doorsInOperation = true;
            CurrentFloor = currentFloor;
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
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

        /// <summary>
        /// Purpose: get and set current floor doors are being animated on
        /// </summary>
        /// <param name="value">current floor index</param>
        /// Purpose: get and set current floor doors are being animated on
        /// Returns: current floor index
        /// -----------------------------------------------------------------
        public int CurrentFloor { get { return currentFloor; } set { currentFloor = value; } }

        /// <summary>
        /// Purpose: get and set elevator direction
        /// </summary>
        /// <param name="value">set elevator direction</param>
        /// Purpose: get and set elevator direction
        /// Returns: string elevator direction
        /// -----------------------------------------------------------------
        public string ElevatorDirection
        {
            get { return direction; }
            set { direction = value; }
        }

        #endregion METHODS

        #region EVENT_HANDLERS

        /// <summary>
        /// Purpose: event handler initiated when doors have been opened
        /// </summary>
        /// <param name="e">EventArgs</param>
        /// Purpose: event handler initiated when doors have been opened
        /// Returns: None
        /// -----------------------------------------------------------------
        public event EventHandler DoorsHaveOpened;
        public async void OndoorsHaveOpened(EventArgs e)
        {
            EventHandler doorsHaveOpened = DoorsHaveOpened;
            if (doorsHaveOpened != null)
            {
                await Task.Run(() => doorsHaveOpened(this, e));
            }
        }

        #endregion EVENT_HANDLERS
    }
}
