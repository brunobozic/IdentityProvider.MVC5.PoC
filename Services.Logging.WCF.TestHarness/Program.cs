using System;
using System.Windows.Forms;

namespace Services.Logging.WCF.TestHarness
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestHarness());
        }
    }
}