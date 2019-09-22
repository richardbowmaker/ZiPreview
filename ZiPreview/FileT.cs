using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace ZiPreview
{
    class Files
    {
        private static List<FileT> files_ = new List<FileT>();

        public static IGuiUpdate GuiUpdateIf { set; private get; }

        public static List<FileT> GetFiles()
        {
            return files_;
        }

        public static void AddFile(string file)
        {
            switch (Path.GetExtension(file).ToLower())
            {
                case ".jpg":
                    AddImageFile(file);
                    break;

                case ".mp4":
                case ".avi":
                    AddVideoFile(file);
                    break;

                case ".lnk":
                    AddLinkFile(file);
                    break;
            }
        }

        private static void AddImageFile(string file)
        {
            // check to see if it already exists, shouldn't happen
            FileT ft = files_.Find(delegate (FileT ft1) 
                { return ft1.ImageFilename.CompareTo(file) == 0; });

            if (ft == null)
            {
                // check to see if an associated video file exists
                ft = files_.Find(delegate (FileT ft1)
                    { return ft1.MatchesVideo(file); });
            }

            if (ft != null)
            {
                ft.ImageFilename = file;
            }
            else
            {
                // check to see if an associated link file exists
                ft = files_.Find(delegate (FileT ft1)
                { return ft1.ImageMatchesLink(file); });

                if (ft != null)
                {
                    ft.ImageFilename = file;
                }
                else
                {
                    FileT f = new FileT();
                    f.ImageFilename = file;
                    files_.Add(f);
                }
            }
        }
        private static void AddVideoFile(string file)
        {
            // check to see if it already exists, shouldn't happen
            FileT ft = files_.Find(delegate (FileT ft1)
                { return ft1.VideoFilename.CompareTo(file) == 0; });

            if (ft == null)
            {
                // check to see if an assocaited video file exists
                ft = files_.Find(delegate (FileT ft1)
                    { return ft1.MatchesImage(file); });

            }

            if (ft != null)
            {
                ft.VideoFilename = file;
            }
            else
            {
                FileT f = new FileT();
                f.VideoFilename = file;
                files_.Add(f);
            }
        }

        private static void AddLinkFile(string file)
        {
            // check to see if it already exists, shouldn't happen
            FileT ft = files_.Find(delegate (FileT ft1)
            { return ft1.VideoFilename.CompareTo(file) == 0; });

            if (ft == null)
            {
                // check to see if an associated image file exists
                ft = files_.Find(delegate (FileT ft1)
                    { return ft1.LinkMatchesImage(file); });
            }

            if (ft != null)
            {
                ft.LinkFilename = file;
            }
            else
            {
                FileT f = new FileT();
                f.LinkFilename = file;
                files_.Add(f);
            }
        }

        public static void AddFiles(string [] files)
        {
            foreach (string file in files) AddFile(file);
        }

        public static void AddFiles(List<string> files)
        {
            foreach (string file in files) AddFile(file);
        }

        public static void Clear()
        {
            files_.Clear();
        }

        // creates an image preview for videos that have no image
        public static void CreateImages()
        {
            foreach (FileT file in files_)
            {
                if (file.HasVideo && !file.HasImage)
                {
                    // get duration of media
                    var wmp = new WMPLib.WindowsMediaPlayer();
                    var media = wmp.newMedia(file.VideoFilename);
                    double duration = media.duration;

                    // get preview image from middle
                    int d = (int)(duration / 2);
                    string dss = (d / 60).ToString() + ":" + (d % 60).ToString();
                    string dsf = (d / 60).ToString() + ":" + ((d + 1) % 60).ToString();

                    // video file name less extension plus "-1.jpg"
                    string fie = Path.GetDirectoryName(file.VideoFilename) + "\\" +
                        Path.GetFileNameWithoutExtension(file.VideoFilename);

                    string args = "-y -ss " + dss + " -to " + dsf + " -i " + 
                        file.VideoFilename + " -frames 1 " + fie + "-%d.jpg";

                    Utilities.RunCommandSync("ffmpeg", args);
                   
                    if (File.Exists(fie + "-1.jpg"))
                    {
                        file.ImageFilename = fie + "-1.jpg";
                        GuiUpdateIf.RefreshGridRowTS(file);
                        GuiUpdateIf.TraceTS("Created image for: " + file.VideoFilename);
                    }
                    else
                    {
                        GuiUpdateIf.TraceTS("*** Failed to create image for: " + file.VideoFilename);
                    }
                }
            }
        }

        public static bool SaveLinkImage(string link, Bitmap bitmap)
        {
            // create a filename pair for the link and the image based on a time stamp

            // create unqiue filename
            // check for folder already exists

            string fn = DateTime.Now.ToString("yyyyMMddHHmmss");

            string ifn;
            string lfn;

            if (Constants.TestMode)
            {
                ifn = Constants.TestDir + "\\f" + fn + ".jpg";
                lfn = Constants.TestDir + "\\f" + fn + ".lnk";
            }
            else
            {
                // to do
            }

            // save link and image
            StreamWriter sw = new StreamWriter(lfn);
            sw.WriteLine(link);
            sw.Close();
            bitmap.Save(ifn, ImageFormat.Jpeg);

            // create file structure and add to list
            FileT file = new FileT();
            file.ImageFilename = ifn;
            file.LinkFilename = lfn;
            files_.Add(file);

            // add to gui
            GuiUpdateIf.AddFileToGridTS(file);

            return true;
        }
    }
    public class FileT
    {
        public FileT()
        {
            ImageFilename = "";
            VideoFilename = "";
            LinkFilename = "";
            LastDate = "";
            Times = "";
            Selected = false;
        }

        public string ImageFilename { get; set; }
        public string VideoFilename { get; set; }
        public string LinkFilename { get; set; }
        public string LastDate { get; set; }
        public string Times { get; set; }
        public bool Selected { get; set; }
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

        public bool MatchesVideo(string ifile)
        {
            if (!HasVideo) return false;
            string ife = Path.GetDirectoryName(ifile) + "\\" + Path.GetFileNameWithoutExtension(ifile);
            string vfe = Path.GetDirectoryName(VideoFilename) + "\\" + Path.GetFileNameWithoutExtension(VideoFilename);
            return ife.CompareTo(vfe + "-1") == 0;
        }
        public bool MatchesImage(string vfile)
        {
            if (!HasImage) return false;
            string vfe = Path.GetDirectoryName(vfile) + "\\" + Path.GetFileNameWithoutExtension(vfile);
            string ife = Path.GetDirectoryName(ImageFilename) + "\\" + Path.GetFileNameWithoutExtension(ImageFilename);
            return ife.CompareTo(vfe + "-1") == 0;
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
    }

    class FileTComparer : System.Collections.IComparer
    {
        private static bool _ascending = true;
        private static string _col = "";

        public FileTComparer(string col)
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
            FileT f1;
            FileT f2;

            if (_ascending)
            {
                f1 = (FileT)((DataGridViewRow)o1).Tag;
                f2 = (FileT)((DataGridViewRow)o2).Tag;
            }
            else
            {
                f2 = (FileT)((DataGridViewRow)o1).Tag;
                f1 = (FileT)((DataGridViewRow)o2).Tag;
            }

            switch (_col)
            {
                case "colFilename"  : return f1.Filename.CompareTo(f2.Filename);
                case "colSelected"  : return f1.Selected.CompareTo(f2.Selected);
                case "colType"      : return f1.TypeS.CompareTo(f2.TypeS);
                case "colTimes"     : return f1.TimesI.CompareTo(f2.TimesI);
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



