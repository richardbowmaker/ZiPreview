using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ZiPreview
{
    class Tryout
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public void Run()
        {
            //DisplayWindows();

            //IntPtr p = FindWindow("Tor Browser");

            //if (p == (IntPtr)0)
            //{
            //    Logger.Error("Window not found");
            //    return;
            //}

            //string s = GetText(p);
            //Logger.Info("Window found: " + s);

            //DisplayChildWindows(p, 0);

            List<IntPtr> ws = GetWindows();
            IntPtr aw = GetActiveWindow();

            foreach (IntPtr w in ws)
            {
                if (w == aw)
                    Logger.Info("Active window: " + w.ToString() + " " + GetText(w));
            }
        }

        public void DisplayWindows()
        {
            List<IntPtr> ws = GetWindows();

            foreach (IntPtr w in ws)
            {
                Logger.Info(w.ToString() + " " + GetText(w));
            }
        }

 
        public string GetText(IntPtr p)
        {
            int chars = 1000;
            StringBuilder buff = new StringBuilder(chars);

            if (GetWindowText(p, buff, chars) > 0)
                return buff.ToString();
            else
                return "";
        }

        public void DisplayChildWindows(IntPtr p, int depth)
        {
            List<IntPtr> ws = GetChildWindows(p);

            foreach (IntPtr w in ws)
            {
                Logger.Info(new String(' ', depth * 4) + w.ToString());
                DisplayChildWindows(w, depth + 1);
            }

        }


        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        /// <summary>
        /// Returns a list of child windows
        /// </summary>
        /// <param name="parent">Parent of the windows to return</param>
        /// <returns>List of child windows</returns>
        public List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }


        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowProc callback, IntPtr i);

        public List<IntPtr> GetWindows()
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumWindows(childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        /// <summary>
        /// Callback method to be used when enumerating windows.
        /// </summary>
        /// <param name="handle">Handle of the next window</param>
        /// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
        /// <returns>True to continue the enumeration, false to bail</returns>
        private bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }

        /// <summary>
        /// Delegate for the EnumChildWindows method
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
        /// <returns>True to continue enumerating, false to bail.</returns>
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);


        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();



        /// <summary>
        /// ///////////////////////////
        /// </summary>
        /// <returns></returns>

        public List<string> GetOpenApps()
        {
            List<string> apps = new List<string>();

            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                    Logger.Info(process.ProcessName + " " + process.Id + " " + process.MainWindowTitle);

                }
            }

            return apps;
        }


        //----------------------
    }
}
