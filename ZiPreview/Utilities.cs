﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ZiPreview
{
    class Utilities
    {
        // runs a script file 
        // - cmd is the command to run, e.g. diskpart or ps
        // - filename is the scriptfile that command invokes
        // - script is the list of commands in the script file
        // this method creates the script file 'filename' and writes the script to it
        // before invoking it with the command 'cmd'
        // note the cmd should include the placeholder <file> for the position of script filename parameter;
        //  e.g diskpart <file> /s will be expanded eventually to cmd /C diskpart scriptfilename /s
        static public bool RunScript(string cmd, string filename, List<string> script)
        {
            StreamWriter sFile = new StreamWriter(filename);
            foreach (string l in script)
            {
                sFile.WriteLine(l);
            }
            sFile.Close();

            // replace filename placeholder in command
            int n = cmd.IndexOf("<file>");
            if (n != -1)
            {
                cmd = cmd.Substring(0, n) + filename + cmd.Substring(n + 6);
            }

            // run script
            bool ok = true;
            try
            {
                ok = RunCommandSync("cmd.exe", "/C " + cmd);
            }
            catch (Exception)
            {
                ok = false;
            }
            return ok;
        }

        // runs a command line and waits for it to finish
        static public bool RunCommandSync(string cmd, string args, int timeout = 30000)
        {
            try
            {
                Process process = new System.Diagnostics.Process();
                ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = true;
                startInfo.FileName = cmd;
                startInfo.Arguments = args;
                process.StartInfo = startInfo;

                if (process.Start())
                {
                    if (process.WaitForExit(timeout))
                    {
                        int ec = process.ExitCode;
                        process.Close();
                        return ec == 0;
                    }
                    process.Close();
                }
                return false;
            }
            catch (Exception)
            {
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
    }
}
