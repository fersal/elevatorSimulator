using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
    public class Elevator
    {
		private const int HOME_FLOOR = 1;
		private const int TRAVEL_TIME = 4000;
		private const int DOOR_TIME = 2000;

		private List<IPeople> passangerList;
		private List<int> floorsAtWhichToStop;
		private int currentFloor;

		public Elevator()
		{
			passangerList = new List<IPeople>();
			floorsAtWhichToStop = new List<int>();
			currentFloor = HOME_FLOOR;
		}

		private bool OpenDoor()
		{
			System.Threading.Thread.Sleep(DOOR_TIME);
			return true;
		}

		private bool CloseDoor()
		{
			System.Threading.Thread.Sleep(DOOR_TIME);
			return true;
		}

		public void TravelToNextStop(int floorStop)
		{
			if (currentFloor > floorStop)
			{
				System.Threading.Thread.Sleep(TRAVEL_TIME);
				currentFloor--;
				TravelToNextStop(floorStop);
			}

			if (currentFloor < floorStop)
			{
				System.Threading.Thread.Sleep(TRAVEL_TIME);
				currentFloor++;
				TravelToNextStop(floorStop);
			}

			currentFloor = floorStop;
		}

		private void BroadcastArrival() { }

		public void SimulateTravel()
		{
			//Might want to use someting like this to have a callback that broadcast that Elevator has arrived at destination floor
			//System.Threading.Timer travelTimer = new System.Threading.Timer();

			System.Threading.Thread.Sleep(TRAVEL_TIME);
		}

		public void CallElevator(int passangerPickupFloor)
		{
			TravelToNextStop(passangerPickupFloor);
		}

		public bool EnterElevator(Passenger newPassanger)
		{
			OpenDoor();
			currentFloor = newPassanger.EntryFloor;
			passangerList.Add(newPassanger);
			return CloseDoor();
		}

		public void ExitElevator() { }

		public int DisplayCurrentFloor()
		{
			return currentFloor;
		}
	}
}
