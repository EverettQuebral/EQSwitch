using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.EQSwitch
{
    public class SwitchStateChangedEventArgs : EventArgs
    {
        public readonly int switchNumber;
        public readonly bool state;

        public SwitchStateChangedEventArgs(int switchNumber, bool state)
        {
            this.switchNumber = switchNumber;
            this.state = state;
        }
    }
}
