using System;
using System.Windows.Forms;

namespace HF_CS_GCS
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
