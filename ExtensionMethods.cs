using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.EQSwitch
{

    public static class ExtensionMethods
    {
        public static void InvokeIfRequired(this CheckBox control, Action<CheckBox> action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => action(control)));
            }
            else
            {
                action(control);
            }
        }
    }
}