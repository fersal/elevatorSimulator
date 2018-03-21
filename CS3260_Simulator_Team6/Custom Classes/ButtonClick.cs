using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3260_Simulator_Team6
{
    public class ButtonClick : IReciever
    {
        RequestPool request_;
        bool floorOneUp_, floorTwoUp_, floorTwoDown_, floorThreeUp_,
            floorThreeDown_, floorFourDown_;
        ACTION_LIST currentAction;

        public ButtonClick(RequestPool request)
        {
            floorOneUp_ = false; floorTwoUp_ = false; floorTwoDown_ = false; floorThreeUp_ = false;
            floorThreeDown_ = false; floorFourDown_ = false;
            request_ = request;

        }

        public bool CLick()
        {
            bool result = false;

            if (currentAction == ACTION_LIST.floorFourDown)
            {
                if (!floorFourDown_)
                {
                    floorFourDown_ = true;
                    request_.AddRequest("Floor 4 Down Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeUp)
            {
                if (!floorThreeUp_)
                {
                    floorThreeUp_ = true;
                    request_.AddRequest("Floor 3 Up Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeDown)
            {
                if (!floorThreeDown_)
                {
                    floorThreeDown_ = true;
                    request_.AddRequest("Floor 3 Down Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoUp)
            {
                if (!floorTwoUp_)
                {
                    floorTwoUp_ = true;
                    request_.AddRequest("Floor 2 Up Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoDown)
            {
                if (!floorTwoDown_)
                {
                    floorTwoDown_ = true;
                    request_.AddRequest("Floor 2 Down Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (currentAction == ACTION_LIST.floorOneUp)
            {
                if (!floorOneUp_)
                {
                    floorOneUp_ = true;
                    request_.AddRequest("Floor 1 Up Request");
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public void SetAction(ACTION_LIST action)
        {
            currentAction = action;
        }

        public void UnClick()
        {
            if (currentAction == ACTION_LIST.floorFourDown)
            {
                if (floorFourDown_)
                {
                    floorFourDown_ = false;
                    request_.CompleteRequest("Floor 4 Down Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeUp)
            {
                if (floorThreeUp_)
                {
                    floorThreeUp_ = false;
                    request_.CompleteRequest("Floor 3 Up Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorThreeDown)
            {
                if (floorThreeDown_)
                {
                    floorThreeDown_ = false;
                    request_.CompleteRequest("Floor 3 Down Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoUp)
            {
                if (floorTwoUp_)
                {
                    floorTwoUp_ = false;
                    request_.CompleteRequest("Floor 2 Up Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorTwoDown)
            {
                if (floorTwoDown_)
                {
                    floorTwoDown_ = false;
                    request_.CompleteRequest("Floor 2 Down Request");
                }
            }
            else if (currentAction == ACTION_LIST.floorOneUp)
            {
                if (floorOneUp_)
                {
                    floorOneUp_ = false;
                    request_.CompleteRequest("Floor 1 Up Request");
                }
            }
        }

        public bool isClicked()
        {
            bool result = false;
            if (currentAction == ACTION_LIST.floorFourDown)
            {
                result = floorFourDown_;
            }
            else if (currentAction == ACTION_LIST.floorThreeUp)
            {
                result = floorThreeUp_;
            }
            else if (currentAction == ACTION_LIST.floorThreeDown)
            {
                result = floorThreeDown_;
            }
            else if (currentAction == ACTION_LIST.floorTwoUp)
            {
                result = floorTwoUp_;
            }
            else if (currentAction == ACTION_LIST.floorTwoDown)
            {
                result = floorTwoDown_;
            }
            else if (currentAction == ACTION_LIST.floorOneUp)
            {
                result = floorOneUp_;
            }
            return result;
        }
    }
}
