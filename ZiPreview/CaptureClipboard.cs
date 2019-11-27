using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace ZiPreview
{
    class CaptureClipboard
    {
        private static string ctext_;

        public static void TimerEvent()
        {
            // on first pass capture what is already in the clipboard
            if (ctext_ == null)
            {
                if (Clipboard.ContainsText())
                {
                    ctext_ = Clipboard.GetText();
                }
                else
                {
                    ctext_ = "";
                }
            }

            if (Clipboard.ContainsText())
            {
                string ctext = Clipboard.GetText();

                if (ctext.StartsWith("https://") && ctext_.CompareTo(ctext) != 0)
                {
                    ctext_ = ctext;
                }
            }

            if (Clipboard.ContainsImage())
            {
                Bitmap bm = (Bitmap)Clipboard.GetData("Bitmap");
                Clipboard.SetText("123", TextDataFormat.Text);

                if (ctext_.Length > 0)
                {
                    FileSetManager.SaveLinkImage(ctext_, bm);
                }
            }
        }
    }



}
