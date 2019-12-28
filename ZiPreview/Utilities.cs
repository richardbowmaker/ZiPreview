using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ZiPreview
{
    class Utilities
    {

        // runs a command line and waits for it to finish
        static public bool RunCommandSync(
            string cmd,
            string wdir,
            string args,
            int timeout = 30000)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = cmd,
                    WorkingDirectory = wdir,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                Process process = new Process();
                process.StartInfo = startInfo;

                if (process.Start())
                {
                    if (process.WaitForExit(timeout))
                    {
                        int ec = process.ExitCode;
                        process.Close();

                        if (ec == 0)
                            return true;
                        else
                        {
                            Logger.Error("Process failed: " + cmd + " " + args);
                            Logger.Error("error code: " + ec.ToString());
                            return false;
                        }
                    }
                    process.Close();
                }
                return false;
            }
            catch (Exception e)
            {
                Logger.Error("Process exception: " + cmd + " " + args);
                Logger.Error("message: " + e.Message);
                return false;
            }
        }

        static public Process LaunchBrowser(FileSet file)
        {
            if (file.HasLink)
            {
                Logger.Info("Launching browser: " + Constants.Browser + ", link: " + file.Link);
                return Process.Start(Constants.Browser, file.Link);
            }
            else
            {
                Logger.Error("Launch browser but no link specified");
                return null;
            }
        }
        static public Process LaunchBrowser()
        {
            Logger.Info("Launching browser: " + Constants.Browser);
            return Process.Start(Constants.Browser);
        }

        static public bool DeleteFile(string fn)
        {
            // delete file, no exceptions
            try
            {
                File.Delete(fn);
                Logger.Info("Deleted file: " + fn);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static public bool MoveFile(string src, string dest)
        {
            if (src.CompareTo(dest) == 0)
            {
                Logger.Error("Attempt to move file to same name: " + src);
                return true;
            }

            // no nonsense move file
            DeleteFile(dest);
            try
            {
                File.Move(src, dest);
                Logger.Info("Moved file " + src + " to " + dest);
                return true;
            }
            catch (Exception)
            {
                Logger.Error("Error moving file " + src + " to " + dest);
                return false;
            }
        }

        static public bool CopyFile(string src, string dest)
        {
            // no nonsense move file
            DeleteFile(dest);
            try
            {
                File.Copy(src, dest);
                Logger.Info("Copied file " + src + " to " + dest);
                return true;
            }
            catch (Exception)
            {
                Logger.Error("Error copying file " + src + " to " + dest);
                return false;
            }
        }
        static public bool WaitForFileReady(string file, int timeout)
        {
            int t = 0;

            while (true)
            {
                try
                {
                    StreamReader sr = new StreamReader(file);
                    sr.Read();
                    sr.Close();
                    return true;
                }
                catch (IOException)
                {
                    if (t >= timeout) return false;
                    System.Threading.Thread.Sleep(250);
                    t += 250;
                }
            }
        }

        public static long? GetDriveFreeSpace(string file)
        {
            long? fs = null;
            DriveInfo drive = new DriveInfo(file);
            if (drive.IsReady) fs = drive.TotalFreeSpace;
            return fs;
        }

        public static string GetFirstUnusedDrive()
        {
            int i = 0;
            while( i < 26)
            {
                string d = (char)(i + 'H') + ":\\";

                try
                {
                    Directory.GetFiles(d);
                }
                catch (Exception)
                {
                    // drive not used
                    return d;
                }

                if (d.CompareTo("Z:\\:") == 0) break;
                ++i;
            }
            return null;
        }

        public static bool MakeDirectory(string path)
        {
            // if the directory doesn't already exists it creates it
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    Logger.Info("Created folder: " + path);
                    return true;
                }
                catch (DirectoryNotFoundException)
                {
                    Logger.Error("Could not create folder: " + path);
                    return false;
                }
            }
            return true;
        }

        public static void CreateFileIfDoesntExist(string file)
        {
            if (!File.Exists(file))
            {
                try
                {
                    File.Create(file);
                    Logger.Info("Created file: " + file);
                }
                catch (DirectoryNotFoundException)
                {
                    Logger.Error("Could not create file: " + file);
                }
            }
        }

        public static string GetDrive(string file)
        {
            int n = file.IndexOf(":");
            if (n == -1)
            {
                Logger.Error("No drive for file: " + file);
                return "";
            }
            else return file.Substring(0, n + 2);
        }

        public static string BytesToString(long bs)
        {
            string s = "";

            long kbs = bs / 1024;
            long mbs = kbs / 1024;
            long gbs = mbs / 1024;

            if (gbs > 0) s = gbs.ToString() + "." + ((mbs % 1024) / 102).ToString() + "GB";
            else if (mbs > 0) s = mbs.ToString() + "." + ((kbs % 1024) / 102).ToString() + "MB";
            else if (kbs > 0) s = kbs.ToString() + "." + ((bs % 1024) / 102).ToString() + "kb";
            else s = bs.ToString() + " bytes";

            s += " (" + bs.ToString("#,##0") + ")";

            return s;
        }

        public static string FilenameNoDriveAndExtension(string file)
        {
            string fne = Path.GetDirectoryName(file) + "\\" +
                         Path.GetFileNameWithoutExtension(file);
            int n = fne.IndexOf(":");
            if (n != -1) fne = fne.Substring(n + 1);
            return fne;
        }
    }
}
