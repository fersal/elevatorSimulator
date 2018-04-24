using System.Windows;
using System.Windows.Controls;

namespace CS3260_Simulator_Team6
{
    class AddNewPassenger
    {
        #region FIELDS

        private int floorIndex;
        private Passenger NewPassenger;
        MainWindow window = (MainWindow)System.Windows.Application.Current.MainWindow;
        WriteToFile WriteLog;

        #endregion FIELDS

        #region METHODS
        /// <summary>
        /// Purpose: AddNewPassenger Constructor
        /// </summary>
        /// <param name="Write">WriteToFile object</param>
        /// Purpose: Constructs AddNewPassenger class
        /// Returns: None
        /// -----------------------------------------------------------------
        public AddNewPassenger(WriteToFile Write)
        {
            WriteLog = Write;
        }

        /// <summary>
        /// Purpose: sets and returns floor index
        /// </summary>
        /// <param name="value">a positive integer value</param>
        /// Purpose: Holds the floor index for the passenger
        /// Returns: Floor index value
        /// -----------------------------------------------------------------
        public int FloorIndex { get { return floorIndex; } set { floorIndex = value; } }

        /// <summary>
        /// Purpose: returns window objects
        /// </summary>
        /// Purpose: Retrieve window objects as needed
        /// Returns: Window objects
        /// -----------------------------------------------------------------
        public MainWindow MyForm { get { return window; } private set { } }

        /// <summary>
        /// Purpose: Returns floor object
        /// </summary>
        /// Purpose: Retrieve floor object
        /// Returns: Floor object
        /// -----------------------------------------------------------------
        public Floor MyFloor
        {
            get { return (MyForm.MyBuilding.ArrayOfAllFloors[this.floorIndex]); }
            private set { }
        }

        /// <summary>
        /// Purpose: Creates/Adds passenger to the floor
        /// </summary>
        /// <param name="destination">destination floor</param>
        /// <param name="personImage">an image of the passenger</param>
        /// Purpose: Create passenger
        /// Returns: None
        /// -----------------------------------------------------------------
        public void AddPassenger(int destination, Image personImage)
        {
            if (MyFloor.GetCurrentAmmountOfPeopleInTheQueue() >= MyFloor.GetMaximumAmmountOfPeopleInTheQueue())
            {
                MessageBox.Show("It looks like the corridor is too crowdy now. Please, wait a while until elevators take few passengers away.", "Your passenger has to wait");
                return;
            }

            NewPassenger = new Passenger(MyForm.MyBuilding, this.MyFloor, destination, personImage, WriteLog);
        }

        /// <summary>
        /// Purpose: Load passenger to the floor
        /// </summary>
        /// Purpose: Send an event for the new passenger on the floor
        /// Returns: None
        /// -----------------------------------------------------------------
        public void LoadPassenger()
        {
            this.MyFloor.OnNewPassengerAppeared(new PassengerEventArgs(NewPassenger));
        }

        #endregion METHODS
    }
}
