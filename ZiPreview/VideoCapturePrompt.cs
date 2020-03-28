using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZiPreview
{
    public partial class VideoCapturePrompt : Form
    {
        public static bool OK = true;
        public static bool Mute = true;
        public static bool Unmute = true;
        public static bool LaunchBrowser = true;
        public static bool CloseBrowser = true;
        private static bool _doNotShow = false;
        private static string _url = "";

        public VideoCapturePrompt()
        {
            InitializeComponent();
        }

        public static bool Run(string url)
        {
            // by pass dialog
            if (_doNotShow) return true;

            _url = url;
            OK = false;
            VideoCapturePrompt form = new VideoCapturePrompt();
            form.ShowDialog();
            return OK;
        }

        private void VideoCapturePrompt_Load(object sender, EventArgs e)
        {
            lblURL.Text = _url;
            MaximumSize = Size;
            MinimumSize = Size;
            textInstructions.Text =
                "1. Close this dialog\r\n" +
                "2. Setup browser to full screen mode\r\n" +
                "3. Press Ctrl-F8 to start recording, there will be a countdown of 5 beeps\r\n" +
                "4. Start browser playing\r\n\r\n" +
                "Record can be stopped with Ctrl-F9\r\n";

            chkMuteDuring.Checked = Mute;
            chkUnmuteAfter.Checked = Unmute;
            chkLaunchBrowser.Checked = LaunchBrowser;
            chkCloseBrowser.Checked = CloseBrowser;
        }

        private void ButOK_Click(object sender, EventArgs e)
        {
            OK = true;
            Close();
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            OK = false;
            Close();
        }

        private void VideoCapturePrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mute = chkMuteDuring.Checked;
            Unmute = chkUnmuteAfter.Checked;
            LaunchBrowser = chkLaunchBrowser.Checked;
            CloseBrowser = chkCloseBrowser.Checked;
        }

        private void ChkDoNotShow_CheckedChanged(object sender, EventArgs e)
        {
            _doNotShow = chkDoNotShow.Checked;
        }
    }
}
