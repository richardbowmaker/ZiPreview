using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZiPreview
{
    class Constants
    {
        public static string WorkingFolder { get; set; }
        public const string Title = "ZiPreview";

        public static string TestDir { get { return WorkingFolder + @"\TestData"; } }

        public const string ObsConnect = "ws://127.0.0.1:4444";
        public const string ObsPassword = "XespePerfo0";

        public const string ObsCapturePath = @"ObsCapture";
        public const string FilesTargetPath = @"Files\All";
        public const string FilesPath = @"Files";
        public static string PropertiesFile = @"\PropertyCache.txt";

        public static string UnmountFile { get { return WorkingFolder + @"\\unmount.bat"; } }

        public static string TestDestFolder1 { get { return WorkingFolder + @"\CopyFolder1"; } }
        public static string TestDestFolder2 { get { return WorkingFolder + @"\CopyFolder2"; } }

        public static string Browser {  get { return WorkingFolder + @"\Tor Browser\Tor - Shortcut.lnk"; ; } }
        
        public static string VeracryptExe
        {
            get => @"C:\Program Files\VeraCrypt\VeraCrypt.exe";
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
        public static long MinimumCaptureSpace = 500L * 1000000L; // 500 MB

        public static string BeepWav { get { return WorkingFolder + @"\Executable\beep.wav"; } }
        public static string BongWav { get { return WorkingFolder + @"\Executable\bong.wav"; } }
        public static string EndedWav { get { return WorkingFolder + @"\Executable\ended.wav"; } }
        public static string ErrorWav { get { return WorkingFolder + @"\Executable\error.wav"; } }
    }
}
