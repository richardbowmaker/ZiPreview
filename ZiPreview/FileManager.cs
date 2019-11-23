using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace ZiPreview
{
    class FileManager
    {
        private static List<FileSet> _files = new List<FileSet>();

        public static List<FileSet> GetFiles()
        {
            return _files;
        }

        // key value pair = <filename, volume description>
        //  
        public static void AddFile(FileVolume file)
        {
            switch (Path.GetExtension(file.Filename).ToLower())
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

        private static void AddImageFile(FileVolume file)
        {
            // check to see if it already exists
            FileSet ft = _files.Find(ft1 => ft1.SameImage(file.Filename));

            if (ft == null)
            {
                // check to see if an associated video file exists
                ft = _files.Find(ft1 => ft1.ImageMatchesVideo(file.Filename));
            }
            else
            {
                // already exists
                return;
            }

            if (ft != null)
            {
                ft.ImageFilename = file.Filename;
            }
            else
            {
                // check to see if an associated link file exists
                ft = _files.Find(ft1 => ft1.ImageMatchesLink(file.Filename));

                if (ft != null)
                {
                    ft.ImageFilename = file.Filename;
                }
                else
                {
                    FileSet f = new FileSet();
                    f.ImageFilename = file.Filename;
                    f.Volume = file.Volume;
                    _files.Add(f);
                }
            }
        }
        private static void AddVideoFile(FileVolume file)
        {
            // check to see if it already exists
            FileSet ft = _files.Find(ft1 => ft1.SameVideo(file.Filename));

            if (ft == null)
            {
                // check to see if an associated video file exists
                ft = _files.Find(ft1 => ft1.VideoMatchesImage(file.Filename));
            }
            else
            {
                return;
            }

            if (ft != null)
            {
                ft.VideoFilename = file.Filename;
            }
            else
            {
                FileSet f = new FileSet();
                f.VideoFilename = file.Filename;
                f.Volume = file.Volume;
                _files.Add(f);
            }
        }

        private static void AddLinkFile(FileVolume file)
        {
            // check to see if it already exists
            FileSet ft = _files.Find(ft1 => ft1.SameLink(file.Filename));

            if (ft == null)
            {
                // check to see if an associated image file exists
                ft = _files.Find(ft1 => ft1.LinkMatchesImage(file.Filename));
            }
            else
            {
                return;
            }

            if (ft != null)
            {
                ft.LinkFilename = file.Filename;
            }
            else
            {
                FileSet f = new FileSet();
                f.LinkFilename = file.Filename;
                f.Volume = file.Volume;
                _files.Add(f);
            }
        }

        public static void AddFiles(List<FileVolume> files)
        {
            files.ForEach(f => AddFile(f));
        }

        public static void Clear()
        {
            _files.Clear();
        }

        // creates an image preview for videos that have no image
        public static void CreateImages()
        {
            foreach (FileSet file in _files)
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
                List<DriveVolume> drives = new List<DriveVolume>();
                drives.AddRange(VeracryptManager.GetDrives());
                drives.AddRange(VhdManager.GetDrives());
                DriveVolume drive = Utilities.FindDriveWithFreeSpace(drives, 250000);

                if (drive == null)
                {
                    MessageBox.Show("Insufficient disk space",
                        Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (!Utilities.MakeDirectory(Constants.FilesTargetPath))
                    return false;

                ifn = drive.Drive + Constants.FilesTargetPath + "\\file" + fn + ".jpg";
                lfn = drive.Drive + Constants.FilesTargetPath + "\\file" + fn + ".lnk";
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
            FileSet file = new FileSet();
            file.ImageFilename = ifn;
            file.LinkFilename = lfn;
            _files.Add(file);

            // add to gui
            frmZiPreview.GuiUpdateIf.AddFileToGridTS(file);

            return true;
        }

        public static void DeleteFile(FileSet file, bool image, bool video, bool link)
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
                int i = _files.FindIndex(delegate (FileSet f) { return f == file; });
                if (i != -1) _files.RemoveAt(i);
                frmZiPreview.GuiUpdateIf.RemoveGridRowTS(file);
            }
        }

        public static bool PopulateFiles(List<DriveVolume> drives)
        {
            SortedList<string, FileVolume> fileList = new SortedList<string, FileVolume>();

            foreach (DriveVolume drive in drives)
            {
                string[] files = {};

                for (int i = 0; i < 10; ++i)
                {
                    try
                    {
                        files = Directory.GetFiles(drive.Drive + Constants.FilesPath, 
                            "*.*", SearchOption.AllDirectories);
                        break;
                    }
                    catch (Exception)
                    {
                    }
                }

                foreach (string file in files)
                {
                    string s = Path.GetDirectoryName(file).Substring(2) + "\\" + Path.GetFileName(file)
                                    + fileList.Count.ToString("0000000");
                    fileList.Add(s, new FileVolume(file, drive.Volume) );
                }
            }

            _files.Clear();
            foreach (KeyValuePair<string, FileVolume> kvp in fileList)
                AddFile(kvp.Value);

            // read the properties
            foreach (DriveVolume drive in drives)
            {
                // read the properties file
                string fn = drive.Drive + Constants.PropertiesFile1;
                Utilities.CreateFileIfNotExist(fn);
                string[] lines = File.ReadAllLines(fn);

                // for each property
                foreach (string line in lines)
                {
                    // parse the property, file;key;value
                    string[] fields = line.Split(';');
                    if (fields.Length == 3)
                    {
                        // find the file set the property belongs to
                        foreach (FileSet fs in _files)
                        {
                            // save the property
                            if (fs.MatchesAny(fields[0]))
                                fs.SetProperty(fields[1], fields[2]);
                        }
                    }
                }
            }
            return _files.Count > 0;
        }

        public static void WriteProperties()
        {
            foreach (VeracryptVolume vol in VeracryptManager.Volumes)
            {
                string fn = vol.Drive + Constants.PropertiesFile1;
                StreamWriter sw = new StreamWriter(fn);

                foreach (FileSet fs in _files)
                {
                    if (fs.DriveMatches(vol.Drive))
                        fs.WriteProperties(sw);
                }

                sw.Close();
            }
        }
    }

    public class FileVolume
    {
        public FileVolume(string filename, string volume)
            { Filename = filename; Volume = volume; }

        public string Filename;
        public string Volume;
    }

    public class DriveVolume
    {
        public DriveVolume(string drive, string volume)
            { Drive = drive; Volume = volume; }

        public string Drive;
        public string Volume;
    }

    
}



