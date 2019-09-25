﻿using System;
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
            _url = url;
            _bitmap = bitmap;
            CheckImageLink form = new CheckImageLink();
            form.ShowDialog();
            return OK;
        }

        public CheckImageLink()
        {
            InitializeComponent();
        }

        private void CheckImageLink_Load(object sender, EventArgs e)
        {
            Size = _size;
            lblUrl.Text = _url;
            _picture = new CPicture(panel);
            _picture.LoadBitmap(_bitmap);
            BringToFront();
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