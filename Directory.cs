using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderIteration
{
    class Folder
    {
        public string path;
        public long size;

        public Folder(string p, long s)
        {
            path = p;
            size = s;
        }
    }
}
