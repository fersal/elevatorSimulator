using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
    public class PassengerEventArgs : EventArgs
    {
        public Passenger PassengerWhoRisedAnEvent;

        public PassengerEventArgs(Passenger PassengerWhoRisedAnEvent)
        {
            this.PassengerWhoRisedAnEvent = PassengerWhoRisedAnEvent;
        }
    }
}
