using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppsExplorer.CustomFunction
{
    public static class searchFolderFile
    {
        public static void getAllDirectory(string path, List<string> folderList)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                folderList.Add(d.FullName);
                getAllDirectory(d.FullName, folderList);
            }
        }
    }
}
