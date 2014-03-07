using System;
using System.Windows.Forms;

namespace HighFlyers.CsGCS
{
    class Utils
    {
        public static void InvokeOrDie(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }
}
