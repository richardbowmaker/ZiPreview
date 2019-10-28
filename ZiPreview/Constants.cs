using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZiPreview
{
    class Constants
    {
        public const bool TestMode = false;

        public static string WorkingFolder { get; set; }
        public const string Title = "ZiPreview";

        public static string TestDir { get { return WorkingFolder + @"\TestData"; } }

        public const string ObsConnect = "ws://127.0.0.1:4444";
        public const string ObsPassword = "XespePerfo0";

        public const string ObsCapturePath = @"ObsCapture";
        public const string FilesTargetPath = @"Files\All";
        public const string FilesPath = @"Files";
        public static string UnmountFile { get { return WorkingFolder + @"\\unmount.bat"; } }

        public static List<string> ScanPaths = new List<string>() { "Encrypted" };

        public static string TestDestFolder1 { get { return WorkingFolder + @"\CopyFolder1"; } }
        public static string TestDestFolder2 { get { return WorkingFolder + @"\CopyFolder2"; } }

        public static string Browser
        {
            get
            {
                if (TestMode) return @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                else return WorkingFolder + @"\Tor Browser\Browser\firefox.exe";
            }
        }

        public static string ObsCaptureDir
        {
            get
            {
                if (TestMode) return WorkingFolder + @"\ObsCapture";
                else return VhdManager.ObsCaptureDir;
            }
        }

        public static string VeracryptExe
        {
            get => @"C:\Program Files\VeraCrypt\VeraCrypt.exe";
        }
        public static string VeracryptFormatExe
        {
            get => @"C:\Program Files\VeraCrypt\VeraCrypt Format.exe";
        }

        private static string _password = "";
        public static string Password
        {
            set
            {
                _password = value;
            }
            get
            {
                if (_password.Length == 0) return "dummypassword";
                else return _password;
            }
        }

        // minimum disk space required for OBS capture
        public static long MinimumCaptureSpace = 20L * 1000000L; // 500 MB

        public static string BeepWav { get { return WorkingFolder + @"\Executable\beep.wav"; } }
        public static string BongWav { get { return WorkingFolder + @"\Executable\bong.wav"; } }
        public static string EndedWav { get { return WorkingFolder + @"\Executable\ended.wav"; } }
        public static string ErrorWav { get { return WorkingFolder + @"\Executable\error.wav"; } }
    }
}
