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
    public interface IEasyMenu
    {
        int GetNoOfOptions();
        string GetOptionText(int n);
        bool GetOptionEnabled(int n);
        void OptionSelected(int n);
    }

    public partial class EasyMenu : Form
    {
        private class EasyOption
        {
            public string Text;
            public bool Enabled;

            override public string ToString()
            {
                return Text;
            }
        }

        private bool keyDown_;
        private long keyDownTime_;
        private long spaceDownTime_;
        private static IEasyMenu callback_;
        private static int lastOption_;

        public EasyMenu()
        {
            InitializeComponent();
        }

        public static void Run()
        {
            if (callback_ == null) return;
            EasyMenu dlg = new EasyMenu();
            dlg.ShowDialog();
        }
        private void EasyMenu_Load(object sender, EventArgs e)
        {
            spaceDownTime_ = 0;
            keyDown_ = false;

            lstOptions.DrawMode = DrawMode.OwnerDrawFixed;
            lstOptions.DrawItem += lstOptions_DrawItem;

            FormBorderStyle = FormBorderStyle.None;

            for (int i = 0; i < callback_.GetNoOfOptions(); i++)
            {
                EasyOption o = new EasyOption();
                o.Text = callback_.GetOptionText(i);
                o.Enabled = callback_.GetOptionEnabled(i);
                lstOptions.Items.Add(o);
            }

            // default to last option selected
            if (lastOption_ < lstOptions.Items.Count)
                lstOptions.SetSelected(lastOption_, true);
            else
                lstOptions.SetSelected(0, true);
        }

        public static void SetCallback(IEasyMenu callback)
        {
            callback_ = callback;
        }

        private void EasyMenu_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void EasyMenu_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void LstOptions_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keyDown_)
            {
                keyDownTime_ = DateTime.Now.Ticks;
                keyDown_ = true;
            }
        }

        private void LstOptions_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                long dt = DateTime.Now.Ticks - keyDownTime_;

                if (dt < 1500000)
                {
                    if (spaceDownTime_ == 0)
                    {
                        spaceDownTime_ = DateTime.Now.Ticks;
                    }
                    else
                    {
                        // double space bar tap
                        spaceDownTime_ = 0;
                        lastOption_ = lstOptions.SelectedIndex;
                        callback_.OptionSelected(lstOptions.SelectedIndex);
                        Close();
                    }
                }
                else if (dt > 5000000)
                {
                    // quit menu if space bar held
                    Close();
                    spaceDownTime_ = 0;
                }
                else
                {
                    lstOptions.SelectedIndex =
                        (lstOptions.SelectedIndex + 1) % lstOptions.Items.Count;
                    spaceDownTime_ = 0;
                }
            }
            else
                // any key but space bar will reset double space bar tap
                spaceDownTime_ = 0;
            keyDown_ = false;
        }

        private static void lstOptions_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            //g.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            ListBox lb = (ListBox)sender;
            EasyOption eo = (EasyOption)lb.Items[e.Index];
            Color c = Color.LightGray;
            if (eo.Enabled) c = Color.Black;
            g.DrawString(lb.Items[e.Index].ToString(), e.Font, new SolidBrush(c), new PointF(e.Bounds.X, e.Bounds.Y));
            e.DrawFocusRectangle();
        }
    }
}
