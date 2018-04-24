using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
	public class Passenger : IPeople
	{
		private const int  WAIT_FOR_ELEVATOR = 1000;

		public string PassangerType { get; set; }
		public int EntryFloor { get; set; }
		public int DestinationFloor { get; set; }
        public Image PassengerImage { get; set; }

		public Passenger(int destinationFloor, int entryFloor, string passangerType, Image passangerImage)
		{
			PassangerType = passangerType;
			DestinationFloor = destinationFloor;
			EntryFloor = entryFloor;
            PassengerImage = passangerImage;
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
