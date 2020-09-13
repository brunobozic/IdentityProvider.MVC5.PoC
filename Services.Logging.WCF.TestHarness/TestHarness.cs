using log4net;
using Services.Logging.WCF.TestHarness.WcfLogServiceReference;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Services.Logging.WCF.TestHarness
{
    public partial class TestHarness : Form
    {
        public TestHarness()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var c = new LogWcfClient();
            var r = new LogToDatabaseRequest
            {
                LoggingEventDto = new LoggingEventDto()
            };
            r.LoggingEventDto.DisplayName = string.Empty;
            r.LoggingEventDto.Domain = string.Empty;
            r.LoggingEventDto.ExceptionString = string.Empty;
            r.LoggingEventDto.Identity = string.Empty;
            r.LoggingEventDto.LoggerName = string.Empty;
            r.LoggingEventDto.RenderedMessage = string.Empty;
            r.LoggingEventDto.ThreadName = string.Empty;
            r.LoggingEventDto.TimeStamp = DateTime.UtcNow;
            r.LoggingEventDto.UserName = string.Empty;
            r.LoggingEventDto.LoggingEventData = new LoggingEventData();

            try
            {
                textBox1.Text += "Attempting to log" + Environment.NewLine;
                c.AppendToLog(r);
                textBox1.Text += "Logging done" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                textBox1.Text += "Error: " + ex.Message + Environment.NewLine;
                ILog logger = LogManager.GetLogger(string.Empty);
                logger.Fatal(ex);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
