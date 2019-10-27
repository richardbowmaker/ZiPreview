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
        private static List<FileT> _files = new List<FileT>();

        public static List<FileT> GetFiles()
        {
            return _files;
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
            // check to see if it already exists
            FileT ft = _files.Find(ft1 => ft1.SameImage(file));

            if (ft == null)
            {
                // check to see if an associated video file exists
                ft = _files.Find(ft1 => ft1.ImageMatchesVideo(file));
            }
            else
            {
                return;
            }

            if (ft != null)
            {
                ft.ImageFilename = file;
            }
            else
            {
                // check to see if an associated link file exists
                ft = _files.Find(ft1 => ft1.ImageMatchesLink(file));

                if (ft != null)
                {
                    ft.ImageFilename = file;
                }
                else
                {
                    FileT f = new FileT();
                    f.ImageFilename = file;
                    _files.Add(f);
                }
            }
        }
        private static void AddVideoFile(string file)
        {
            // check to see if it already exists
            FileT ft = _files.Find(ft1 => ft1.SameVideo(file));

            if (ft == null)
            {
                // check to see if an associated video file exists
                ft = _files.Find(ft1 => ft1.VideoMatchesImage(file));
            }
            else
            {
                return;
            }

            if (ft != null)
            {
                ft.VideoFilename = file;
            }
            else
            {
                FileT f = new FileT();
                f.VideoFilename = file;
                _files.Add(f);
            }
        }

        private static void AddLinkFile(string file)
        {
            // check to see if it already exists
            FileT ft = _files.Find(ft1 => ft1.SameLink(file));

            if (ft == null)
            {
                // check to see if an associated image file exists
                ft = _files.Find(ft1 => ft1.LinkMatchesImage(file));
            }
            else
            {
                return;
            }

            if (ft != null)
            {
                ft.LinkFilename = file;
            }
            else
            {
                FileT f = new FileT();
                f.LinkFilename = file;
                _files.Add(f);
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
            _files.Clear();
        }

        // creates an image preview for videos that have no image
        public static void CreateImages()
        {
            foreach (FileT file in _files)
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
                        frmZiPreview.GuiUpdateIf.RefreshGridRowTS(file);
                        Logger.Info("Created image for: " + file.VideoFilename);
                    }
                    else
                    {
                        Logger.Error("*** Failed to create image for: " + file.VideoFilename);
                    }
                }
            }
        }

        public static bool SaveLinkImage(string link, Bitmap bitmap)
        {
            // check the user wishes to save the link and image
            if (!CheckImageLink.Verify(link, bitmap)) return false;

            // create a filename pair for the link and the image based on a time stamp

            // create unqiue filename
            // check for folder already exists

            string fn = DateTime.Now.ToString("yyyyMMddHHmmss");
            string ifn = "";
            string lfn = "";

            if (Constants.TestMode)
            {
                ifn = Constants.TestDir + "\\file" + fn + ".jpg";
                lfn = Constants.TestDir + "\\file" + fn + ".lnk";
            }
            else
            {
                // link and image should only need 250kb max
                List<string> drives = VeracryptManager.GetDrives();
                string drive = Utilities.FindDriveWithFreeSpace(drives, 250000);

                if (drive == null)
                {
                    MessageBox.Show("Insufficient disk space",
                        Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (!Utilities.MakeDirectory(Constants.FilesTargetPath))
                    return false;

                ifn = drive + Constants.FilesTargetPath + "\\file" + fn + ".jpg";
                lfn = drive + Constants.FilesTargetPath + "\\file" + fn + ".lnk";
            }

            // save link
            StreamWriter sw = new StreamWriter(lfn);
            sw.WriteLine(link);
            sw.Close();
            Logger.Info("Saved link \"" + link + "\" to file: " + lfn);

            // save image
            bitmap.Save(ifn, ImageFormat.Jpeg);
            Logger.Info("Saved image to file: " + ifn);

            // create file structure and add to list
            FileT file = new FileT();
            file.ImageFilename = ifn;
            file.LinkFilename = lfn;
            _files.Add(file);

            // add to gui
            frmZiPreview.GuiUpdateIf.AddFileToGridTS(file);

            return true;
        }

        public static void DeleteFile(FileT file, bool image, bool video, bool link)
        {
            if (image)
            {
                Utilities.DeleteFile(file.ImageFilename);
                file.ImageFilename = "";
            }
            if (video)
            {
                Utilities.DeleteFile(file.VideoFilename);
                file.VideoFilename = "";
            }
            if (link)
            {
                Utilities.DeleteFile(file.LinkFilename);
                file.LinkFilename = "";
            }

            if (file.HasImage || file.HasVideo || file.HasLink)
            {
                frmZiPreview.GuiUpdateIf.RefreshGridRowTS(file);
            }
            else
            {
                int i = _files.FindIndex(delegate (FileT f) { return f == file; });
                if (i != -1) _files.RemoveAt(i);
                frmZiPreview.GuiUpdateIf.RemoveGridRowTS(file);
            }
        }

        public static bool PopulateFiles(List<string> drives)
        {
            SortedList<string, string> fileList = new SortedList<string, string>();

            foreach (string drive in drives)
            {
                string[] files = {};

                for (int i = 0; i < 10; ++i)
                {
                    try
                    {
                        files = Directory.GetFiles(drive + Constants.FilesPath, 
                            "*.*", SearchOption.AllDirectories);
                        break;
                    }
                    catch (Exception)
                    {
                    }
                }

                foreach (string fn in files)
                {
                    string s = Path.GetDirectoryName(fn).Substring(2) + "\\" + Path.GetFileName(fn)
                                    + fileList.Count.ToString("0000000");
                    fileList.Add(s, fn);
                }
            }

            foreach (KeyValuePair<string, string> kvp in fileList)
                AddFile(kvp.Value);

            return _files.Count > 0;
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



