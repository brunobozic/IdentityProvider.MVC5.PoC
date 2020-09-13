using Services.Logging.WCF.TestHarness.WcfLogServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            WcfLogServiceReference.LogWcfClient c = new WcfLogServiceReference.LogWcfClient();
            LogToDatabaseRequest r = new LogToDatabaseRequest();
            r.LoggingEventDto = new LoggingEventDto();
            r.LoggingEventDto.DisplayName = "";
            r.LoggingEventDto.Domain = "";
            r.LoggingEventDto.ExceptionString = "";
            r.LoggingEventDto.Identity = "";
            r.LoggingEventDto.LoggerName = "";
            r.LoggingEventDto.RenderedMessage = "";
            r.LoggingEventDto.ThreadName = "";
            r.LoggingEventDto.TimeStamp = DateTime.UtcNow;
            r.LoggingEventDto.UserName = "";
            r.LoggingEventDto.LoggingEventData = new LoggingEventData();

            try
            {
                textBox1.Text += textBox1.Text + "Attempting to log" + Environment.NewLine;
                c.AppendToLog(r);
            }
            catch (Exception ex)
            {
                textBox1.Text += textBox1.Text + "Error: " + ex.Message + Environment.NewLine; 
          
            }
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
