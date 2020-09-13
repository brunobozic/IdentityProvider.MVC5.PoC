using System;
using System.Windows.Forms;

namespace Services.Logging.WCF.TestHarness
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestHarness());
        }
    }
}
