using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ZiPreview
{
    class Logger
    {
        public enum LoggerLevel
        {
            Info,
            Error
        }

        public class LoggerItem
        {
            private string _text;
            private LoggerLevel _level;

            public LoggerItem(string text, LoggerLevel level)
            {
                _text = text;
                _level = level;
            }

            public string Text { get => _text; }
            public LoggerLevel Level { get => _level; }

            public override string ToString()
            {
                return _text;
            }
        }

        private static ListBox _listBox = null;
        public static LoggerLevel Level { get; set; }

        public static ListBox TheListBox
        {
            get => _listBox;
            set
            {
                _listBox = value; 
                _listBox.DrawMode = DrawMode.OwnerDrawFixed;
                _listBox.DrawItem += listBox_DrawItem;
            }
        }

        private delegate void VoidStringType(string str, LoggerLevel level);

        public static void TraceInfo(string text)
        {
            if (Level > LoggerLevel.Info) return; 

            if (TheListBox.InvokeRequired)
            {
                VoidStringType action = new VoidStringType(Trace);
                _listBox.Invoke(action, new object[] { text, LoggerLevel.Info });
            }
            else
            {
                Trace(text, LoggerLevel.Info);
            }
        }
        public static void TraceError(string text)
        {
            if (TheListBox.InvokeRequired)
            {
                VoidStringType action = new VoidStringType(Trace);
                _listBox.Invoke(action, new object[] { text, LoggerLevel.Error });
            }
            else
            {
                Trace(text, LoggerLevel.Error);
            }
        }

        private static void Trace(string text, LoggerLevel level)
        {
            _listBox.Items.Add(new LoggerItem(text, level));
            MakeLastLineVisible();
        }

        private static void MakeLastLineVisible()
        {
            int topIndex = _listBox.Items.Count -
                (_listBox.Height / _listBox.GetItemHeight(0));
            if (topIndex > 0) _listBox.TopIndex = topIndex;
        }

        private static void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            //g.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            ListBox lb = (ListBox)sender;
            LoggerItem li = (LoggerItem)lb.Items[e.Index];
            Color c = Color.Black;
            if (li.Level == LoggerLevel.Error) c = Color.Red;
            g.DrawString(lb.Items[e.Index].ToString(), e.Font, new SolidBrush(c), new PointF(e.Bounds.X, e.Bounds.Y));
            e.DrawFocusRectangle();
        }
    }
}
