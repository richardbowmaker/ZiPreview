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

        private int _totalWidth;        // width in pixels of progress bar
        private List<Part> _parts;
        private int _totalSize;         // total of all part sizes

        private int _part;              // current part
        public int _min;                // minimum value of part
        public int _max;
        private int _partWidth;         // part size in pixels
        private int _pos;               // current position of progress bar
        private int _value;             // current value of the current part

        class Part
        {
            public Part(string description, int size)
            {
                Size = size;
                Description = description;
                MinPos = 0;
            }

            public int Size;
            public string Description;      // description of part shown next to progress bar
            public int MinPos;              // position in rogress bar of min value
        }

        public MultiPartProgressBar(int width)
        {
            _totalWidth = width;
            _parts = new List<Part>();
            _part = -1;
        }

        private void UpdateProgress()
        {
            //string s = String.Format("Bar width {0}, total sizes {1}, part {2}, part min pos {3}, part width {4}, value {5}, pos {6}",
            //    _totalWidth, _totalSize, _part, _parts[_part].MinPos, _partWidth, _value, _pos);
            //Logger.Info(s);

            ZipPreview.GUI.UpdateProgressTS(_pos, _parts[_part].Description);
        }

        public int AddPart(string description, int size)
        {
            _parts.Add(new Part(description, size));
            _totalSize += size;
            return _parts.Count();
        }

        public int NextStage(int min, int max)
        {
            if (_part >= _parts.Count()) Clear();
            else
            {
                _min = min;
                _max = max;
                _part++;
                _partWidth = (_totalWidth * _parts[_part].Size) / _totalSize;

                // calculate position of min value of part within the whole progress bar
                int s = 0;
                for (int i = 0; i < _part; i++) s += _parts[i].Size;
                _pos = _parts[_part].MinPos;
                _value = _min;

                //@ update progress bar
                UpdateProgress();
            }
            return _part;
        }

        public void Start(int min, int max)
        {
            _part = -1;
            _pos = 0;
            _value = 0;

            // calculate starting position of each part
            int s = 0;
            for (int i = 0; i < _parts.Count(); i++)
            {
                _parts[i].MinPos = (_totalWidth * s) / _totalSize;
                s += _parts[i].Size;
            }

            NextStage(min, max);
        }

        public void Clear()
        {
            _parts = new List<Part>();
            _part = -1;
            _pos = 0;
            _value = 0;
            _totalSize = 0;

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
            int pos = _parts[_part].MinPos +
                ((value - _min) * _partWidth / (_max - _min));

            // return if no update required
            if (pos == _pos) return;

            //@ update progress bar
            _pos = pos;
            UpdateProgress();
        }

        public int IncValue()
        {
            if (_value < _max)
                SetValue(_value + 1);
            return _value;
        }

        public static void Test()
        {
            MultiPartProgressBar pbh = ZipPreview.GUI.GetProgressBar();

            Logger.Info("ProgressBarHelper tests");
            pbh.Clear();

            Logger.Info("*** Test 1 ***");
            pbh.AddPart("Test 1", 1);
            pbh.Start(1, 10);
            for (int i = 1; i <= 10; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(250);
            }
            pbh.Clear();

            Logger.Info("*** Test 2 ***");
            pbh.AddPart("Test 2", 1);
            pbh.Start(1, 10);
            for (int i = 1; i <= 10; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(250);
            }
            pbh.Clear();

            Logger.Info("*** Test 3 ***");
            pbh.AddPart("Test 3", 10);
            pbh.Start(10, 25);
            for (int i = 10; i <= 25; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(200);
            }
            pbh.Clear();

            Logger.Info("*** Test 4 ***");
            pbh.AddPart("Test 4", 10);
            pbh.Start(1, 100);
            for (int i = 1; i <= 100; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(25);
            }
            pbh.Clear();

            Logger.Info("*** Test 5 ***");
            pbh.AddPart("Test 5 Part 1", 1);
            pbh.AddPart("Test 5 Part 2", 1);
            pbh.Start(1, 10);
            for (int i = 1; i <= 10; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(250);
            }
            pbh.NextStage(1, 10);
            for (int i = 1; i <= 10; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(250);
            }
            pbh.Clear();

            Logger.Info("*** Test 6 ***");
            pbh.AddPart("Test 6 Part 1", 1);
            pbh.AddPart("Test 6 Part 2", 2);
            pbh.Start(1, 10);
            for (int i = 1; i <= 10; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(250);
            }
            pbh.NextStage(1, 10);
            for (int i = 1; i <= 10; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(250);
            }
            pbh.Clear();

            Logger.Info("*** Test 7 ***");
            pbh.AddPart("Test 7 Part 1", 1);
            pbh.AddPart("Test 7 Part 2", 3);
            pbh.Start(1, 10);
            for (int i = 1; i <= 10; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(250);
            }
            pbh.NextStage(100, 150);
            for (int i = 100; i <= 150; i++)
            {
                pbh.SetValue(i);
                Thread.Sleep(25);
            }
            pbh.Clear();
        }
    }
}