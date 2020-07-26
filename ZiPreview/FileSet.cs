using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

// old - \Files\All\file20110102030031;times;2;lasttime;28/12/2019;maxvol;-2.5;duration;9:28
// new - a01;averagevol;2.20;duration;03:25:45.678;lasttime;15:52:59 28/03/2020;maxvol;1.10;selected;X;times;2
// \Files\All\f1;maxvol;-0.0  -18.5;duration;00:02:53.000;times;1;lasttime;12:00:00 28/12/2019;volume;-0.0  -18.5


namespace ZiPreview
{
    public class FileSet
    {
        public string ImageFilename { get; set; }
        public string VideoFilename { get; set; }
        public string LinkFilename { get; set; }
        public string Volume { get; set; } // filename of the veracrypt file
        public string LastDate 
        { 
            get 
            { 
                string d = GetProperty("lasttime"); 
                if (d.Length == 10)
                {
                    // upgrade to date time stamp
                    d = "12:00:00 " +  d;
                    SetProperty("lasttime", d);
                }
                return d;
            } 
            set { SetProperty("lasttime", value); }
        }
        public string Times 
        { 
            get { return GetProperty("times"); } 
            set { SetProperty("times", value); }
        }
        public bool Selected
        { 
            get { return GetProperty("selected").CompareTo("X") == 0; }
            set { SetProperty("selected", value ? "X" : ""); }
        }
        public string VolumeDb
        {
            // maxvol;-0.0  -18.5
            get 
            { 
                string d = GetProperty("volume");

                int n1 = d.IndexOf(" ");
                int n2 = d.LastIndexOf(" ");
                if (n1!= -1)
                {
                    string mv = d.Substring(0, n1);
                    string av = d.Substring(n2 + 1);
                    SetProperty("maxvol", mv);
                    SetProperty("averagevol", av);
                }
                return d;
            }
            set { SetProperty("volume", value); }
        }
        public string Duration
        {
            // 03:25:45.678
            // 9:28
            get
            {
                string d = GetProperty("duration");
                if (d.Length == 4)
                {
                    d = "00:0" + d + ".000";
                    SetProperty("duration", d);
                }
                if (d.Length == 5)
                {
                    d = "00:" + d + ".000";
                    SetProperty("duration", d);
                }
                return d;
            }
            set { SetProperty("duration", value); }
        }

        public List<Property> Properties { get; set; }
    
        public FileSet()
        {
            Properties = new List<Property>();
            ImageFilename = "";
            VideoFilename = "";
            LinkFilename = "";
            Volume = "";
            Selected = false;
        }

        public DataGridViewRow Row { get; set; }
        public bool HasVideo { get { return VideoFilename.Length > 0; } }
        public bool HasImage { get { return ImageFilename.Length > 0; } }
        public bool HasLink { get { return LinkFilename.Length > 0; } }

        public string Filename
        {
            get
            {
                if (HasImage) return ImageFilename;
                if (HasVideo) return VideoFilename;
                if (HasLink) return LinkFilename;
                return "";
            }
        }

        public string ShortFilename
        {
            get
            {
                string f = Filename;

                if (f.Length < 30)
                {
                    return f;
                }
                else
                {
                    return f.Substring(0, 3) + "..." + f.Substring(f.Length - 24);
                }
            }
        }

        public string FilenameNoPathAndExt { get { return Path.GetFileNameWithoutExtension(Filename); } }

        public string Link
        {
            get
            {
                if (HasLink)
                {
                    StreamReader sr = new StreamReader(LinkFilename);
                    string link = sr.ReadLine();
                    sr.Close();
                    return link;
                }
                else
                {
                    return "";
                }
            }
        }

        public string TypeS
        {
            get
            {
                string s = "";
                if (HasVideo) s += "V";
                if (HasImage) s += "I";
                if (HasLink) s += "L";
                return s;
            }
        }

        public string SelectedS { get { return GetProperty("selected"); } }

        public int TimesI { get { return Times.Length == 0 ? 0 : Int32.Parse(Times);  } }

        public void ToggleSelected() { Selected = !Selected; }

        public bool FileMatchesAny(string file)
        {
            if (FilesMatch(file, ImageFilename)) return true;
            if (FilesMatch(file, VideoFilename)) return true;
            if (FilesMatch(file, LinkFilename)) return true;
            return false;
        }

        public bool FilesMatch(string f1, string f2)
        {
            if (f1.Length == 0 || f2.Length == 0) return false;
            string fe1 = Utilities.FilenameNoDriveAndExtension(f1);
            string fe2 = Utilities.FilenameNoDriveAndExtension(f2);
            if (fe1.EndsWith("-1")) fe1 = fe1.Substring(0, fe1.Length - 2);
            if (fe2.EndsWith("-1")) fe2 = fe2.Substring(0, fe2.Length - 2);
            return fe1.CompareTo(fe2) == 0;
        }

        // move files to a different volume, drive letter
        public bool MoveFilesToVolume(string drive)
        {
            bool ok = true;
            string idest = "";
            string ldest = "";
            string vdest = "";

            if (HasImage)
            {
                idest = drive + ImageFilename.Substring(3);
                if (VeracryptManager.IsMountedVolume(idest))
                    ok = Utilities.MoveFile(ImageFilename, idest);
                else
                    ok = false;
            }
            if (ok && HasLink)
            {
                ldest = drive + LinkFilename.Substring(3);
                if (VeracryptManager.IsMountedVolume(ldest))
                    ok = Utilities.MoveFile(LinkFilename, ldest);
                else
                    ok = false;
            }
            if (ok && HasVideo)
            {
                vdest = drive + VideoFilename.Substring(3);
                if (VeracryptManager.IsMountedVolume(vdest))
                    ok = Utilities.MoveFile(VideoFilename, vdest);
                else
                    ok = false;
            }

            if (ok)
            {
                VeracryptManager.SetVolumeDirty(drive);
                VeracryptManager.SetVolumeDirty(ImageFilename);

                // files copied ok, update the class attributes
                // and delete the original files
                if (HasImage)
                {
                    Utilities.DeleteFile(ImageFilename);
                    ImageFilename = idest;
                }
                if (HasLink)
                {
                    Utilities.DeleteFile(LinkFilename);
                    LinkFilename = ldest;
                }
                if (HasVideo)
                {
                    Utilities.DeleteFile(VideoFilename);
                    VideoFilename = vdest;
                }
            }
            else
            {
                Logger.Error("FileT: could not move image/link/video files");

                // one of the copies failed, so unwind
                Utilities.DeleteFile(idest);
                Utilities.DeleteFile(ldest);
                Utilities.DeleteFile(vdest);
            }
            return ok;
        }

        // matches the filename less extension and drive against
        // any of the image/link/video
        public bool MatchesAnyFilename(string file)
        {
            string fs = Utilities.FileStem(file);
            if (fs.EndsWith("-1")) fs = fs.Substring(0, fs.Length - 2);

            if (HasImage)
            {
                string f = Utilities.FileStem(ImageFilename);
                if (f.EndsWith("-1")) f = f.Substring(0, f.Length - 2);
                return (String.Compare(fs, f, true) == 0);
            }
            if (HasVideo) return (String.Compare(fs, Utilities.FileStem(VideoFilename), true) == 0);
            if (HasLink) return (String.Compare(fs, Utilities.FileStem(LinkFilename), true) == 0);

            return false;
        }

        /// ///////////////////////////////////////////////////
        /// Properties
        /// ///////////////////////////////////////////////////
        /// 

        public class Property
        {
            public Property()
            {
                Key = "";
                Value = "";
            }
            public Property(string k, string v)
            {
                Key = k;
                Value = v;
            }

            private string key;
            public string Key
            {
                get { return key; }
                set { key = value.ToLower(); }
            }
            public string Value;

            public bool Equals(Property other)
            {
                return Key.CompareTo(other.Key) == 0;
            }

            public bool Equals(string key)
            {
                return Key.CompareTo(key.ToLower()) == 0;
            }
        }

        public string SetProperty(string key, string value)
        {
            if (value.Length == 0)
            {
                // remove property if set to null
                int n = Properties.FindIndex(p => p.Equals(key));
                if (n != -1)
                {
                    Properties.RemoveAt(n);
                    //VeracryptManager.SetVolumeDirty(Filename);
                }
                return "";
            }
            else
            {
                Property p = CreateProperty(key);
                p.Value = value;
                //VeracryptManager.SetVolumeDirty(Filename);
                return value;
            }
        }
        public void LoadProperty(string key, string value)
        {
            Property p = CreateProperty(key);
            p.Value = value;
        }

        public string GetProperty(string key)
        {
            int n = Properties.FindIndex(p => p.Equals(key));

            if (n != -1)
            {
                return Properties[n].Value;
            }
            else
            {
                return "";
            }
        }

        public Property CreateProperty(string key)
        {
            int n = Properties.FindIndex(p => p.Equals(key));

            if (n == -1)
            {
                Property p = new Property(key, "");
                Properties.Add(p);
                n = Properties.Count - 1;
            }
            return Properties[n];
        }
        public string IncCount(string key)
        {
            Property p = CreateProperty(key);
            int c = 0;
            if (p.Value.Length > 0)
            {
                c = Convert.ToInt32(p.Value);
            }
            p.Value = (++c).ToString();
            return p.Value;
        }

        public int GetCount(string key)
        {
            return Convert.ToInt32(GetProperty(key));
        }

        public string SetDateStamp(string key)
        {
            return SetProperty(key, DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy"));
        }

        public string GetDateStamp(string key)
        {
            return GetProperty(key);
        }

        public void WriteProperties(StreamWriter sw)
        {
            if (Properties.Count == 0) return;
            string fn = Utilities.FileStem(Filename);
            // remove trailing -1 if it is image filename
            if (fn.EndsWith("-1")) fn = fn.Substring(0, fn.Length - 2);
            sw.Write(fn);
            foreach (Property p in Properties)
            {
                string v = p.Value.Trim();
                if (v.Length > 0) sw.Write(";" + p.Key + ";" + v);
            }
            sw.WriteLine("");
        }

        // drive letter including colon
        public bool DriveMatches(string drive)
        {
            string fn = Filename;
            int n = fn.IndexOf(":");
            if (n > 0)
            {
                return drive.ToLower().CompareTo(fn.Substring(0, n + 2).ToLower()) == 0;
            }
            else return false;
        }

        public long GetSize()
        {
            long size = 0;
            if (HasImage)
            {
                FileInfo fi = new FileInfo(ImageFilename);
                size += fi.Length;
            }
            if (HasVideo)
            {
                FileInfo fi = new FileInfo(VideoFilename);
                size += fi.Length;
            }
            if (HasLink)
            {
                FileInfo fi = new FileInfo(LinkFilename);
                size += fi.Length;
            }
            return size;
        }

        public static void PropertyTests()
        {
            FileSet fs = new FileSet();

            Logger.Assert(fs.GetProperty("p1"), "", "PropertyTests 1");
            Logger.Assert(fs.GetProperty("p1"), "", "PropertyTests 2");
            Logger.Assert(fs.SetProperty("p1", "123"), "123", "PropertyTests 3");
            Logger.Assert(fs.GetProperty("p1"), "123", "PropertyTests 4");
            Logger.Assert(fs.SetProperty("p2", "456"), "456", "PropertyTests 5");
            Logger.Assert(fs.IncCount("p2"), "457", "PropertyTests 7");
            Logger.Assert(fs.GetProperty("p2"), "457", "PropertyTests 8");
            Logger.Assert(fs.SetProperty("p1", "88"), "88", "PropertyTests 9");
            Logger.Assert(fs.GetProperty("p1"), "88", "PropertyTests 10");
        }
    }
}
