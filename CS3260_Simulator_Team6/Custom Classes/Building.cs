using System;
using System.Collections.Generic;

namespace CS3260_Simulator_Team6
{
    public class Building
    {
        #region FIELDS

        private Floor[] arrayOfAllFloors; //list of all floors (needed e.g. for NewPassengerButtons)
        public Floor[] ArrayOfAllFloors
        {
            get { return arrayOfAllFloors; }
            private set { }
        }

        private Elevator[] arrayOfAllElevators; //list of all elvators
        public Elevator[] ArrayOfAllElevators
        {
            get { return arrayOfAllElevators; }
            private set { }
        }

        private int exitLocation;
        public int ExitLocation
        {
            get { return exitLocation; }
            private set { }
        }

        public List<Passenger> ListOfAllPeopleWhoNeedAnimation;

        public ElevatorManager ElevatorManager;

        #endregion FIELDS


        #region METHODS

        public Building(Doors doors)
        {
            //Set exitLocation on 0 floor
            exitLocation = 677;

            //Initialize floors
            arrayOfAllFloors = new Floor[4];
            arrayOfAllFloors[0] = new Floor(this, 0);
            arrayOfAllFloors[1] = new Floor(this, 1);
            arrayOfAllFloors[2] = new Floor(this, 2);
            arrayOfAllFloors[3] = new Floor(this, 3);

            //Initialize elevators (each elevator starts on randomly choosen floor)
            arrayOfAllElevators = new Elevator[3];
            Random random = new Random();

            arrayOfAllElevators[0] = new Elevator(this, arrayOfAllFloors[random.Next(arrayOfAllFloors.Length)], doors);

            //Initialize list of all people inside (to track who's inside and need to be animated)
            ListOfAllPeopleWhoNeedAnimation = new List<Passenger>();

            //Initialize ElevatorManager object
            ElevatorManager = new ElevatorManager(ArrayOfAllElevators, ArrayOfAllFloors);
        }

        #endregion METHODS
    }
}
