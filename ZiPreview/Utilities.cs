using System;
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

        static public bool RunCommandSync(string cmd, string args)
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
                    if (process.WaitForExit(30000))
                    {
                        process.Close();
                        return true;
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

        static public Process LaunchBrowser(FileT file)
        {
            if (file.HasLink)
            {
                return Process.Start(Constants.Browser, file.Link);
            }
            else
            {
                return null;
            }
        }
    }
}
