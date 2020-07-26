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
    public partial class CheckImageLink : Form
    {
        public static bool OK;
        private static string _url;
        private static Bitmap _bitmap;
        private static Size _size;

        private CPicture _picture;

        static CheckImageLink()
        {
            _size = new Size(600, 600);
            OK = false;
        }

        public static bool Verify(string url, Bitmap bitmap)
        {
            IntPtr hwnd = Utilities.GetActiveWindow();
            _url = url;
            _bitmap = bitmap;
            CheckImageLink form = new CheckImageLink();
            form.ShowDialog();
            Utilities.SetActiveWindow(hwnd);
            return OK;
        }

        public CheckImageLink()
        {
            InitializeComponent();
        }

        private void CheckImageLink_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " Save Image and Link";
            Size = _size;
            MaximumSize = Size;
            MinimumSize = Size;
            lblUrl.Text = _url;
            _picture = new CPicture(panel);
            _picture.LoadBitmap(_bitmap);

            ZipPreview.GUI.SetToTopWindow();
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
    }
}
