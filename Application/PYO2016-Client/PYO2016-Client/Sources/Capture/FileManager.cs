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

        private static int idx = 0;

        private FileManager() {
            k = new LinkedList<KeyValuePair<string, string>>();
        }

        private LinkedList<KeyValuePair<string, string>> k;

        static public FileManager getInstance()
        {
            if (f == null)
                f = new FileManager();
            return f;
        }

        public string addValue(string date, string value)
        {
            date = "[" + idx.ToString() + "] " + date;
            ++idx;

            k.AddLast(new KeyValuePair<string, string>(date, value));
            return date;
        }

        public string getValue(string date)
        {
            foreach (KeyValuePair<string, string> t in k)
            {
                if (t.Key == date)
                {
                    return t.Value;
                }
            }
            return null;
        }

        public void removeValue(int n)
        {
            int a = 0;
            for (var i = k.First; i != null; i = i.Next)
            { 
                if (a == n)
                {
                    k.Remove(i);
                    return;
                }
                ++a;
            }
            return;
        }

        public void clear()
        {
            idx = 0;
            k.Clear();
        }
    }
}
