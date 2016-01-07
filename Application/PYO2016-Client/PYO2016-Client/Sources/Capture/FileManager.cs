using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYO2016_Client.Sources.Capture
{
    class FileManager
    {
        private static FileManager f = null;

        private FileManager() { }

        private LinkedList<KeyValuePair<string, string>> k;

        static public FileManager getInstance()
        {
            if (f == null)
                f = new FileManager();
            return f;
        }

        public bool addValue(string date, string value)
        {
            foreach(KeyValuePair<string, string> t in k)
            {
                if(t.Key == date)
                {
                    return false;
                }
            }
            k.AddLast(new KeyValuePair<string, string>(date, value));
            return true;
        }

        public string getValue(string date)
        {
            foreach(KeyValuePair<string, string> t in k)
            {
                if(t.Key == date)
                {
                    return t.Value;
                }
            }
            return null;
        }

        public void clear()
        {
            k.Clear();
        }
    }
}
