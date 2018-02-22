using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
	class Passanger : IPeople
	{
		private string PassangerType;
		private int EntryFloor;
		private int DestinationFloor;

		public Passanger (int destinationFloor, int entryFloor, string passangerType)
		{
			this.PassangerType = passangerType;
			this.DestinationFloor = destinationFloor;
			this.EntryFloor = entryFloor;
		}

		public string getPassangerType()
		{
			return PassangerType;
		}

		public int getEntryFloor()
		{
			return EntryFloor;
		}

		public int getDestinationFloor()
		{
			return DestinationFloor;
		}
	}
}
