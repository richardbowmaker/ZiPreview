using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ZiPreview
{
    public class FileSet
    {
        public string ImageFilename { get; set; }
        public string VideoFilename { get; set; }
        public string LinkFilename { get; set; }
        public string Volume { get; set; }
        public string LastDate { get; set; }
        public string Times { get; set; }
        public bool Selected { get; set; }

        public List<Property> Properties { get; set; }

        public FileSet()
        {
            ImageFilename = "";
            VideoFilename = "";
            LinkFilename = "";
            Volume = "";
            LastDate = "";
            Times = "";
            Selected = false;
            Properties = new List<Property>();
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

        public string SelectedS
        {
            get
            {
                if (Selected) return "X"; else return "";
            }
        }

        public int TimesI
        {
            get
            {
                if (Times.Length == 0) return 0; else return Int32.Parse(Times);
            }
        }

        public void ToggleSelected()
        {
            Selected = !Selected;
        }

        public bool SameImage(string ifile)
        {
            // test for the same image possible on a different volume
            if (!HasImage) return false;
            string if1 = ifile.Substring(3); // remove drive letter
            string if2 = ImageFilename.Substring(3);
            return if1.CompareTo(if2) == 0;
        }

        public bool SameVideo(string vfile)
        {
            // test for the same image possible on a different volume
            if (!HasVideo) return false;
            string vf1 = vfile.Substring(3); // remove drive letter
            string vf2 = VideoFilename.Substring(3);
            return vf1.CompareTo(vf2) == 0;
        }

        public bool SameLink(string lfile)
        {
            // test for the same image possible on a different volume
            if (!HasLink) return false;
            string lf1 = lfile.Substring(3); // remove drive letter
            string lf2 = LinkFilename.Substring(3);
            return lf1.CompareTo(lf2) == 0;
        }

        public bool ImageMatchesVideo(string ifile)
        {
            if (!HasVideo) return false;
            string ife = Path.GetDirectoryName(ifile) + "\\" + Path.GetFileNameWithoutExtension(ifile);
            string vfe = Path.GetDirectoryName(VideoFilename) + "\\" + Path.GetFileNameWithoutExtension(VideoFilename);
            return ife.CompareTo(vfe + "-1") == 0;
        }

        public bool VideoMatchesImage(string vfile)
        {
            if (!HasImage) return false;
            string vfe = Path.GetDirectoryName(vfile) + "\\" + Path.GetFileNameWithoutExtension(vfile);
            string ife = Path.GetDirectoryName(ImageFilename) + "\\" + Path.GetFileNameWithoutExtension(ImageFilename);
            if (ife.CompareTo(vfe + "-1") == 0) return true;
            if (ife.CompareTo(vfe) == 0) return true;
            return false;
        }

        public bool LinkMatchesImage(string lfile)
        {
            if (!HasImage) return false;
            string lfe = Path.GetDirectoryName(lfile) + "\\" + Path.GetFileNameWithoutExtension(lfile);
            string ife = Path.GetDirectoryName(ImageFilename) + "\\" + Path.GetFileNameWithoutExtension(ImageFilename);
            return ife.CompareTo(lfe) == 0;
        }

        public bool ImageMatchesLink(string ifile)
        {
            if (!HasLink) return false;
            string ife = Path.GetDirectoryName(ifile) + "\\" + Path.GetFileNameWithoutExtension(ifile);
            string lfe = Path.GetDirectoryName(LinkFilename) + "\\" + Path.GetFileNameWithoutExtension(LinkFilename);
            return ife.CompareTo(lfe) == 0;
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
                ok = Utilities.CopyFile(ImageFilename, idest);
            }
            if (ok && HasLink)
            {
                ldest = drive + LinkFilename.Substring(3);
                ok = Utilities.MoveFile(LinkFilename, ldest);
            }
            if (ok && HasVideo)
            {
                vdest = drive + VideoFilename.Substring(3);
                ok = Utilities.MoveFile(VideoFilename, vdest);
            }

            if (ok)
            {
                // files copied ok, update the class attributes
                // and delee theoriginal files
                Utilities.DeleteFile(ImageFilename);
                ImageFilename = idest;
                Utilities.DeleteFile(LinkFilename);
                LinkFilename = ldest;
                Utilities.DeleteFile(VideoFilename);
                VideoFilename = vdest;
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
        public bool MatchesAny(string file)
        {
            string fne = FilenameNoDriveAndExtensionLc(file);
            if (HasImage)
            {
                string f = FilenameNoDriveAndExtensionLc(ImageFilename);
                if (f.EndsWith("-1")) f = f.Substring(0, f.Length - 2);
                return (fne.CompareTo(f) == 0);
            }
            if (HasVideo) return (fne.CompareTo(FilenameNoDriveAndExtensionLc(VideoFilename)) == 0);
            if (HasLink) return (fne.CompareTo(FilenameNoDriveAndExtensionLc(LinkFilename)) == 0);
            return false;
        }

        private string FilenameNoDriveAndExtensionLc(string file)
        {
            string fne = Path.GetDirectoryName(file) + "\\" +
                        Path.GetFileNameWithoutExtension(file);
            fne = fne.ToLower();
            int n = fne.IndexOf(":");
            if (n != -1) fne = fne.Substring(n + 1);
            return fne;
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
            Property p = CreateProperty(key);
            p.Value = value;
            return value;
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
            return SetProperty(key, DateTime.Now.ToString("dd/MM/yyyy"));
        }

        public string GetDateStamp(string key)
        {
            return GetProperty(key);
        }

        public void WriteProperties(StreamWriter sw)
        {
            foreach (Property p in Properties)
            {
                string fn = Filename;
                int n = fn.IndexOf(":");
                if (n > 0) fn = fn.Substring(n + 1);
                sw.WriteLine(fn + ";" + p.Key + ";" + p.Value);
            }
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

    class FileSetComparer : System.Collections.IComparer
    {
        private static bool _ascending = true;
        private static string _col = "";

        public FileSetComparer(string col)
        {
            if (col.CompareTo(_col) == 0)
            {
                _ascending = !_ascending;
            }
            else
            {
                _ascending = true;
            }
            _col = col;
        }

        public int Compare(object o1, object o2)
        {
            FileSet f1;
            FileSet f2;

            if (_ascending)
            {
                f1 = (FileSet)((DataGridViewRow)o1).Tag;
                f2 = (FileSet)((DataGridViewRow)o2).Tag;
            }
            else
            {
                f2 = (FileSet)((DataGridViewRow)o1).Tag;
                f1 = (FileSet)((DataGridViewRow)o2).Tag;
            }

            switch (_col)
            {
                case "colFilename": return f1.Filename.CompareTo(f2.Filename);
                case "colSelected": return f1.Selected.CompareTo(f2.Selected);
                case "colType": return f1.TypeS.CompareTo(f2.TypeS);
                case "colTimes": return f1.TimesI.CompareTo(f2.TimesI);
                case "colDate":
                    string d1 = f1.LastDate;
                    string d2 = f2.LastDate;
                    if (d1.Length == 0) return -1;
                    if (d2.Length == 0) return +1;
                    // dd/mm/yyyy
                    return (d1.Substring(6, 4) + d1.Substring(3, 2) + d1.Substring(0, 2))
                                .CompareTo(d2.Substring(6, 4) + d2.Substring(3, 2) + d2.Substring(0, 2));
            }
            return 0;
        }
    }
}
