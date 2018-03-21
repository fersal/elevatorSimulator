using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
    public class FloorSecondDownCommand : AControlsSystem
    {
        public FloorSecondDownCommand(IReciever reciever) : base(reciever) { }
        public override bool Execute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoDown);
            return reciever_.CLick();
        }
        public override void UnExecute()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoDown);
            reciever_.UnClick();
        }

        public override bool CheckClick()
        {
            reciever_.SetAction(ACTION_LIST.floorTwoDown);
            return reciever_.isClicked();
        }
    }
}
