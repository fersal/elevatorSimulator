using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
    public abstract class AControlsSystem
    {
        protected IReciever reciever_ = null;
        public AControlsSystem(IReciever reciever)
        {
            this.reciever_ = reciever;
        }

        public abstract bool Execute();

        public abstract void UnExecute();

        public abstract bool CheckClick();
    }
}
