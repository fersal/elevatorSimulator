using System.Windows;
using System.Windows.Controls;

namespace CS3260_Simulator_Team6
{
    class AddNewPassenger
    {
        private int floorIndex;
        private Passenger NewPassenger;
        MainWindow window = (MainWindow)Application.Current.MainWindow;
        WriteToFile WriteLog;

        public AddNewPassenger(WriteToFile Write)
        {
            WriteLog = Write;
        }

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

            NewPassenger = new Passenger(MyForm.MyBuilding, this.MyFloor, destination, personImage, WriteLog);
        }


        public void LoadPassenger()
        {
            this.MyFloor.OnNewPassengerAppeared(new PassengerEventArgs(NewPassenger));
        }
    }
}
