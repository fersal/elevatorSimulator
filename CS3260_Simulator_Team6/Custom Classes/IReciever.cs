using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
    public enum ACTION_LIST
    {
        floorFourDown,
        floorThreeUp, floorThreeDown,
        floorTwoUp, floorTwoDown,
        floorOneUp
    }
    public interface IReciever
    {
        void SetAction(ACTION_LIST action);
        bool CLick();
        void UnClick();
        bool isClicked();
    }
}
