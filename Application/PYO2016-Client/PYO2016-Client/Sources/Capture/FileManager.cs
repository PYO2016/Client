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

        private LinkedList<string> filenames;

        public FileManager getInstance()
        {
            if (f == null)
                f = new FileManager();
            return f;
        }

        public bool addFileName(string s)
        {
            foreach(string t in filenames)
            {
                if(t == s)
                {
                    return false;
                }
            }
            filenames.AddLast(s);
            return true;
        }

        public void clear()
        {
            filenames.Clear();
        }

        public LinkedList<string> getFilenames()
        {
            return filenames;
        }
    }
}
