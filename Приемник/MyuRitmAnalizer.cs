using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Приемник
{
    public class MyuRitmAnalizer
    {
        public enum MoveCommand
        {
            Stop, Forward, Left, Right, None
        }

        public static MoveCommand Evaluate(double leftChannelPower, double rightChannelPower,
            double CutOffLeft, double CutOffRight, bool leftInverted, bool rightInverted)
        {
            if (leftChannelPower >= CutOffLeft && rightChannelPower >= CutOffRight)
            {
                if (!leftInverted & !rightInverted)
                    return MoveCommand.Stop;
                else if (leftInverted & rightInverted)
                    return MoveCommand.Forward;
                else if (!leftInverted & rightInverted)
                    return MoveCommand.Right;
                else
                    return MoveCommand.Left;
            }
            else if (leftChannelPower >= CutOffLeft && rightChannelPower < CutOffRight)
            {
                if (!leftInverted & !rightInverted)
                    return MoveCommand.Right;
                if (leftInverted & rightInverted)
                    return MoveCommand.Left;
                else if (!leftInverted & rightInverted)
                    return MoveCommand.Stop;
                return MoveCommand.Forward;
            }  
            else if (leftChannelPower < CutOffLeft && rightChannelPower >= CutOffRight)
            {
                if (!leftInverted & !rightInverted)
                    return MoveCommand.Left;
                if (leftInverted & rightInverted)
                    return MoveCommand.Right;
                else if (!leftInverted & rightInverted)
                    return MoveCommand.Forward;
				return MoveCommand.Stop;
            }  
            else {
                if (!leftInverted & !rightInverted)
                    return MoveCommand.Forward;
                else if (leftInverted & rightInverted)
                    return MoveCommand.Stop;
                else if (!leftInverted & rightInverted)
                    return MoveCommand.Left;
                else
                    return MoveCommand.Right;
            }
        }
    }
}
