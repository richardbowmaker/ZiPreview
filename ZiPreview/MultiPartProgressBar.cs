using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZiPreview
{
    public class MultiPartProgressBar
    {
        // multi part progress bar
        // subdivided into parts each part with a size that represents the proportion of the
        // progress bar it uses. e.g 3 parts with sizes 1, 2 and 2 would split the progress bar into 
        // two sections in the ratio 1:1:2

        // # = part 1, @ = part 2, * = part 3
        //
        // <-                  -                   > total width in pixels                  = _totalWidth
        //                     v                     current part start position in pixels = _partMinPos
        //                     <-           ->       current part width in pixels          = _partWidth
        // ####################@@@@@@@@@@@@@@@****** multi part progress bar (3 parts)
        //

        private int _totalWidth;             // width in pixels of progress bar
        private List<Part> _parts;
        private int _totalSize;             // total of all part sizes

        private int _part;             // current part
        private int _partMinPos;       // position of progress bar that corresponds to the minimum value of the current part
        private int _partWidth;        // part size in pixels
        private int _pos;               // current position of progress bar
        private int _value;             // current value of the current part

        struct Part
        {
            public Part(int min, int max, string description, int size)
            {
                Min = min;
                Max = max;
                Description = description;
                Size = size;
            }

            public int Min;                // minimum value of part
            public int Max;
            public string Description;     // description of part shown next to progress bar
            public int Size;
        }

        public MultiPartProgressBar(int width)
        {
            _totalWidth = width;
            _parts = new List<Part>();
            _part = -1;
        }

        private void UpdateProgress()
        {
            string s = String.Format("Bar width {0}, total sizes {1}, part {2}, part min pos {3}, part width {4}, value {5}, pos {6}", 
                _totalWidth, _totalSize, _part, _partMinPos, _partWidth, _value, _pos);
            Logger.Info(s);

            ZipPreview.GUI.UpdateProgressTS(_pos, _parts[_part].Description);
        }

        public int AddStage(int min, int max, string description, int size)
        {
            _parts.Add(new Part(min, max, description, size));
            _totalSize += size;
            return _parts.Count();
        }

        public int NextStage()
        {
            if (_part >= _parts.Count()) End();
            else
            {
                _part++;
                _partWidth = (_totalWidth * _parts[_part].Size) / _totalSize;

                // calculate position of min value of part within the whole progress bar
                int s = 0;
                for (int i = 0; i < _part; i++) s += _parts[i].Size;
                _partMinPos = (_totalWidth * s) / _totalSize;
                _pos = _partMinPos;
                _value = _parts[_part].Min;

                //@ update progress bar
                UpdateProgress();
            }
            return _part;
        }

        public int Start()
        {
            _part = -1;
            _pos = 0;
            _value = 0;
            return NextStage();
        }

        public void End()
        {
            _parts = new List<Part>();
            _part = -1;
            _pos = 0;
            _value = 0;

            // zero progress bar
            ZipPreview.GUI.UpdateProgressTS(0, "");
        }

        public void SetValue(int value)
        {
            // return if value not changed
            if (value == _value) return;
            _value = value;

            Part part = _parts[_part];

            // calculate new position
            int pos = _partMinPos + 
                ((value - part.Min) * _partWidth / (part.Max - part.Min));

            // return if no update required
            if (pos == _pos) return;

            //@ update progress bar
            _pos = pos;
            UpdateProgress();
        }

        public int IncValue()
        {
            if (_value < _parts[_part].Max)
                SetValue(_value + 1);
            return _value;
        }

        public static void Test()
        {
            MultiPartProgressBar pbh = ZipPreview.GUI.GetProgressBar();

            Logger.Info("ProgressBarHelper tests");
            Logger.Info("*** Test 1 ***");

//            pbh = new MultiPartProgressBar(10);
            pbh.AddStage(1, 10, "", 1);
            pbh.Start();
            for (int i = 0; i < 10; i++)
            {
                pbh.SetValue(i + 1);
                Thread.Sleep(250);
            }

            //Logger.Info("*** Test 2 ***");
            //pbh = new MultiPartProgressBar(50);
            //pbh.AddStage(1, 10, "", 1);
            //pbh.Start();
            //for (int i = 0; i < 10; i++) pbh.SetValue(i + 1);

            //Logger.Info("*** Test 3 ***");
            //pbh = new MultiPartProgressBar(100);
            //pbh.AddStage(10, 25, "", 10);
            //pbh.Start();
            //for (int i = 10; i <= 25; i++) pbh.SetValue(i);

            //Logger.Info("*** Test 4 ***");
            //pbh = new MultiPartProgressBar(10);
            //pbh.AddStage(1, 100, "", 10);
            //pbh.Start();
            //for (int i = 1; i <= 100; i++) pbh.SetValue(i);

            //Logger.Info("*** Test 5 ***");
            //pbh = new MultiPartProgressBar(100);
            //pbh.AddStage(1, 10, "", 1);
            //pbh.AddStage(1, 10, "", 1);
            //pbh.Start();
            //for (int i = 1; i <= 10; i++) pbh.SetValue(i);
            //pbh.NextStage();
            //for (int i = 1; i <= 10; i++) pbh.SetValue(i);

            //Logger.Info("*** Test 6 ***");
            //pbh = new MultiPartProgressBar(100);
            //pbh.AddStage(1, 10, "", 1);
            //pbh.AddStage(1, 10, "", 2);
            //pbh.Start();
            //for (int i = 1; i <= 10; i++) pbh.SetValue(i);
            //pbh.NextStage();
            //for (int i = 1; i <= 10; i++) pbh.SetValue(i);

            //Logger.Info("*** Test 7 ***");
            //pbh = new MultiPartProgressBar(100);
            //pbh.AddStage(1, 10, "", 1);
            //pbh.AddStage(100, 150, "", 3);
            //pbh.Start();
            //for (int i = 1; i <= 10; i++) pbh.SetValue(i);
            //pbh.NextStage();
            //for (int i = 100; i <= 150; i++) pbh.SetValue(i);
        }
    }
}
