using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
    public class FloorFirstUpCommand : AControlsSystem
    {
        public FloorFirstUpCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorOneUp);
            return reciever_.CLick();
        }
        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorOneUp);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorOneUp);
            return reciever_.isClicked();
        }
    }
}
