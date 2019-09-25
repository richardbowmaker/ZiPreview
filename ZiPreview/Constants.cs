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
        public const bool TestMode = false;
        public const string TestDir = @"D:\_Ricks\c#\ZiPreview\TestData";

        public const string ObsConnect = "ws://127.0.0.1:4444";
        public const string ObsPassword = "XespePerfo0";

        public const string ObsCapturePath = @"\ObsCapture";
        public const string FilesTargetPath = @"\Files\All";

        public static string Browser
        {
            get
            {
                if (TestMode) return @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                else return @"D:\tor\Tor Browser\Browser\firefox.exe";
            }
        }

        public static string ObsCaptureDir
        {
            get
            {
                if (TestMode) return @"D:\_Ricks\c#\ZiPreview\ObsCapture";
                else return ManageVHDs.ObsCaptureDir;
            }
        }

        public static string FilesTargetDir
        {
            get
            {
                if (TestMode) return TestDir;
                else return ManageVHDs.FilesTargetDir;
            }
        }
    }
}
