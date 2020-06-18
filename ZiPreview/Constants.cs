using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// audio adjustment
//  https://superuser.com/questions/323119/how-can-i-normalize-audio-using-ffmpeg



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
        public static string PropertiesFile = @"\PropertyCache.txt";

        public static string UnmountFile { get { return WorkingFolder + @"\unmount.bat"; } }

        public static string Browser {  get { return WorkingFolder + @"\Tor Browser\Start Tor Browser.lnk"; } }
 //       public static string Browser {  get { return @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"; } }
        
        public static string VeracryptExe { get { return @"C:\Program Files\VeraCrypt\VeraCrypt.exe"; } }
        public static string FfmpegExe { get { return WorkingFolder + @"\ffmpeg.exe"; } }

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
        public static long MinimumCaptureSpace = 500_000_000L; // 500MB
        public static long MinimumLinkSpace = 250_000L; // 250kb

        public static string BeepWav { get { return WorkingFolder + @"\Executable\beep.wav"; } }
        public static string BongWav { get { return WorkingFolder + @"\Executable\bong.wav"; } }
        public static string EndedWav { get { return WorkingFolder + @"\Executable\ended.wav"; } }
        public static string ErrorWav { get { return WorkingFolder + @"\Executable\error.wav"; } }
    }
}
