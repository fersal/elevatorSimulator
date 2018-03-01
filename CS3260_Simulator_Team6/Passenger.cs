using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
	public class Passenger : IPeople
	{
		private const int  WAIT_FOR_ELEVATOR = 1000;

		public string PassangerType { get; set; }
		public int EntryFloor { get; set; }
		public int DestinationFloor { get; set; }

		public Passenger(int destinationFloor, int entryFloor, string passangerType)
		{
			PassangerType = passangerType;
			DestinationFloor = destinationFloor;
			EntryFloor = entryFloor;
		}
		
		public long UseElevator(Elevator elevator)
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			elevator.CallElevator(EntryFloor);
			while(elevator.DisplayCurrentFloor() != EntryFloor)
			{
				System.Threading.Thread.Sleep(WAIT_FOR_ELEVATOR);
			}

			if (elevator.EnterElevator(this))
			{
				elevator.TravelToNextStop(DestinationFloor);
			}

			while (elevator.DisplayCurrentFloor() != DestinationFloor)
			{
				System.Threading.Thread.Sleep(WAIT_FOR_ELEVATOR);
			}

			stopWatch.Stop();
			return stopWatch.ElapsedMilliseconds / 1000;
		}
	}
}
