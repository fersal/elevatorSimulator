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

        /// <summary>
        /// Purpose: AControlsSystem Constructor
        /// </summary>
        /// <param name="reciever">IReciever object</param>
        /// Purpose: Constructs AControlsSystem class
        /// Returns: None
        /// -----------------------------------------------------------------
        public AControlsSystem(IReciever reciever)
        {
            this.reciever_ = reciever;
        }

        public abstract bool Execute();

        public abstract void UnExecute();

        public abstract bool CheckClick();
    }
}
