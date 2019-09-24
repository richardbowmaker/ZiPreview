using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZiPreview
{
    class Constants
    {
        public const string Title = "ZiPreview";
        public const bool TestMode = true;
        public const string TestDir = @"D:\_Rick's\c#\ZiPreview\TestData";

        public const string ObsConnect = "ws://127.0.0.1:4444";
        public const string ObsPassword = "XespePerfo0";

        public const string ObsCapturePath = @"\ObsCapture";

        public static string Browser
        {
            get
            {
                if (TestMode) return @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                else return @"tor";
            }
        }

        public static string ObsCaptureDir
        {
            get
            {
                if (TestMode) return TestDir + ObsCapturePath;
                else return ManageVHDs.ObsCaptureDir;
            }
        }
    }
}
