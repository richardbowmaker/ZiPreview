using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZiPreview
{
	public class PropertyCache
	{
		private List<string> _dirs;
		private static string _cacheFile = @"\PropertyCache.txt";

		public class PropertyItem
		{
			public string FilePath { get; set; }
			public string Key { get; set; }
			public string Value { get; set; }

			public bool Equals(string fp, string key)
			{
				return (FilePath == fp && Key == key);
			}

			public bool Equals(PropertyItem other)
			{
				return Equals(other.FilePath, other.Key);
			}
		}

		private List<PropertyItem> _properties;

		public PropertyCache()
		{
			_dirs = new List<string>();
			_properties = new List<PropertyItem>();
		}

		public void AddDirectory(string dir)
		{
			if (_dirs.Find(delegate (string s) { return s.Equals(dir); }) == null)
			{
				_dirs.Add(dir);

				// create the cache
				if (!File.Exists(dir + _cacheFile))
				{
					StreamWriter sr = new StreamWriter(dir + _cacheFile, false);
					sr.Close();
				}

				// read the cache
				ReadProperties(dir + _cacheFile);
			}
		}

		private void ReadProperties(string path)
		{
			string du = path.Substring(0, 2).ToUpper();
			string[] lines = File.ReadAllLines(path);
			List<PropertyItem> pis = new List<PropertyItem>();

			foreach (string line in lines)
			{
				string[] tokens = line.Split(';');

				if (tokens.Length == 3)
				{
					PropertyItem pi = new PropertyItem
					{
						FilePath = du + tokens[0],
						Key = tokens[1],
						Value = tokens[2]
					};

                    if (pis.Find(delegate (PropertyItem pi1) { return pi.Equals(pi1); }) == null)
					{
						pis.Add(pi);
					}
				}
			}
			AddProperties(pis);
		}

		public void WriteProperties()
		{
			foreach (string dir in _dirs)
			{
				StreamWriter sr = new StreamWriter(dir + _cacheFile, false);
				foreach (PropertyItem pi in _properties)
				{
					if (pi.FilePath.StartsWith(dir))
					{
						sr.WriteLine(pi.FilePath.Substring(2) + ";" + // remove drive letter
								   pi.Key + ";" +
								   pi.Value);
					}
				}
				sr.Close();
			}
		}

		private void AddProperties(List<PropertyItem> properties)
		{
			foreach (PropertyItem pi in properties)
			{
				if (_properties.Find(delegate (PropertyItem pi1) { return pi.Equals(pi1); }) == null)
				{
					_properties.Add(pi);
				}
			}
		}

		public string SetProperty(string file, string key, string value)
		{
			PropertyItem pi = CreateProperty(file, key);
			pi.Value = value;
            return value;
        }

        public string GetProperty(string file, string key)
        {
            PropertyItem pi = _properties.Find(delegate (PropertyItem pi1) { return pi1.Equals(file, key); });

            if (pi != null)
            {
                return pi.Value;
            }
            else
            {
                return "";
            }
        }

        public PropertyItem CreateProperty(string file, string key)
		{
			PropertyItem pi = _properties.Find(delegate (PropertyItem pi1) { return pi1.Equals(file, key); });

			if (pi == null)
			{
				pi = new PropertyItem
				{
					FilePath = file,
					Key = key,
					Value = ""
				};
				_properties.Add(pi);
			}
			return pi;
		}

		public string IncCount(string file, string key)
		{
			PropertyItem pi = CreateProperty(file, key);
			int c = 0;
			if (pi.Value.Length > 0)
			{
				c = Convert.ToInt32(pi.Value);
			}
			pi.Value = (++c).ToString();
            return pi.Value;
		}

		public int GetCount(string file, string key)
		{
			return Convert.ToInt32(GetProperty(file, key));
		}

        public string DateStamp(string file, string key)
        {
            return SetProperty(file, key, DateTime.Now.ToString("dd/MM/yyyy"));
        }

        public string GetDateStamp(string file, string key)
        {
            return GetProperty(file, key);
        }

    }
}
