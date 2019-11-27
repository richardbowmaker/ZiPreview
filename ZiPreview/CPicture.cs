using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;



namespace ZiPreview
{
    class CPicture
    {
        private Panel      _panel;
        private PictureBox _picBox;

        private Point      _mouseDown;
        private bool       _bIgnoreClick;

        private int _nScale;
        private int _nTop;
        private int _nLeft;

        private const int   _nMouseMoveDelta = 10;
        private const float _fScaleInc       = 1.25f;
        public CPicture(Panel p_panel)
        {
            _panel = p_panel;
            _picBox = new PictureBox();
            _panel.Controls.Add(_picBox);

            _picBox.SizeMode = PictureBoxSizeMode.Zoom;
            _picBox.Top = 0;
            _picBox.Left = 0;
            _picBox.Width = _panel.Width;
            _picBox.Height = _panel.Height;
            _picBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | 
                AnchorStyles.Left | AnchorStyles.Right;

            _picBox.MouseClick +=
                new System.Windows.Forms.MouseEventHandler(this.picBox_MouseClick);
            _picBox.MouseDown +=
                new System.Windows.Forms.MouseEventHandler(this.picBox_MouseDown);
            _picBox.MouseUp +=
                new System.Windows.Forms.MouseEventHandler(this.picBox_MouseUp);
            _picBox.MouseMove +=
                new System.Windows.Forms.MouseEventHandler(this.picBox_MouseMove);

            _bIgnoreClick = false;

            _nLeft = 0;
            _nTop = 0;
            _nScale = 0;
        }

        public void LoadFile(string file)
        {
            _picBox.SizeMode = PictureBoxSizeMode.Zoom;
            _picBox.Top = 0;
            _picBox.Left = 0;
            _picBox.Width = _panel.Width;
            _picBox.Height = _panel.Height;

            _picBox.ImageLocation = file;

            _nScale = 0; 
        }

        public void Uninitialise()
        {
            _picBox.ImageLocation = "";
        }
        public void LoadBitmap(Bitmap bitmap)
        {
            _picBox.SizeMode = PictureBoxSizeMode.Zoom;
            _picBox.Top = 0;
            _picBox.Left = 0;
            _picBox.Width = _panel.Width;
            _picBox.Height = _panel.Height;

            _picBox.Image = bitmap;
   
            _nScale = 0;
        }

        public void ZeroZoomSettings()
        {
            try
            {
                float fScale = 1.0f;

                if (_nScale > 0)
                {
                    while (_nScale-- > 0) fScale = fScale / _fScaleInc;
                }
                else if (_nScale < 0)
                {
                    while (_nScale++ < 0) fScale = fScale * _fScaleInc;
                }

                if (fScale != 1.0f)
                {
                    _picBox.Scale(new SizeF(fScale, fScale));
                }

                _nLeft = 0;
                _nTop = 0;
                _nScale = 0;

                _picBox.Left = _nLeft;
                _picBox.Top  = _nTop;
                _picBox.Refresh();
            }
            catch (Exception)
            {
            }
        }

        private void picBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_bIgnoreClick)
            {
                _bIgnoreClick = false;
                return;
            }


            if (e.Button == MouseButtons.Left)
            {
                UpdatePicture(
                      (_panel.Width  / 2) - (int)(_fScaleInc * e.X),
                      (_panel.Height / 2) - (int)(_fScaleInc * e.Y),
                      1);
            }
            if (e.Button == MouseButtons.Right)
            {
                if (_nScale > 0)
                {
                    UpdatePicture(
                          (_panel.Width  / 2) - (int)(e.X / _fScaleInc),
                          (_panel.Height / 2) - (int)(e.Y / _fScaleInc),
                          -1);
                }
            }

        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDown = new Point(e.X, e.Y);
            }
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_mouseDown != null)
                {
                    int nDx = e.X - _mouseDown.X;
                    int nDy = e.Y - _mouseDown.Y;

                    if (nDx > _nMouseMoveDelta ||
                        nDx < -_nMouseMoveDelta ||
                        nDy > _nMouseMoveDelta ||
                        nDy < -_nMouseMoveDelta)
                    {
                        UpdatePicture(
                              _picBox.Left + nDx,
                              _picBox.Top + nDy,
                              0);
                        _bIgnoreClick = true;
                    }
                }
            }
        }

        private void UpdatePicture(int nX, int nY, int nScale)
        {
            try
            {
                _nLeft = nX;
                _nTop = nY;

                float fScale = 1.0f;

                if (nScale > 0)
                {
                    _nScale += nScale;
                    while (nScale-- > 0) fScale = fScale * _fScaleInc;
                }
                else if (nScale < 0)
                {
                    _nScale += nScale;
                    while (nScale++ < 0) fScale = fScale / _fScaleInc;
                }

                if (fScale != 1.0f)
                {
                    _picBox.Scale(new SizeF(fScale, fScale));
                }

                _picBox.Left = _nLeft;
                _picBox.Top = _nTop;
                _picBox.Refresh();
            }
            catch (Exception)
            {
            }
        }

        public string ZoomSettings
        {
            get
            {
                if (_nLeft == 0 && _nTop == 0 && _nScale == 0)
                {
                    return "";
                }
                else
                {
                    return _nLeft.ToString() + "," +
                           _nTop.ToString() + "," +
                           _nScale.ToString();
                }
            }
        } 

    }
}
