using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace AppsExplorer.CustomFunction
{
    public static class pathConverter
    {
        public static string getRelativePath(string URI1,string URI2)
        {
            //返回URI1对URI2的相对路径
            //两个参数都必须是绝对路径，否则返回null
            if (!(Path.IsPathRooted(URI1) && Path.IsPathRooted(URI2))) return null;
            Uri uri1 = new Uri(URI1);
            Uri uri2 = new Uri(URI2);
            Uri relativeUri = uri2.MakeRelativeUri(uri1); 
            return Uri.UnescapeDataString(relativeUri.ToString());
        }
        public static string getAbsolutePath(string xURI,string baseURI)
        {
            //返回xURI对于baseURI的绝对路径
            //如果xURI不是相对路径，则返回null
            if (Path.IsPathRooted(xURI) || (!Path.IsPathRooted(baseURI))) return null;
            Uri baseUri = new Uri(baseURI);
            Uri absoluteUri = new Uri(baseUri,xURI);
            return Uri.UnescapeDataString(absoluteUri.AbsolutePath);
        }
        public static string getAbsolutePathDefault(string xURI)
        {
            //基准固化为：程序所在目录
            return getAbsolutePath(xURI, System.AppDomain.CurrentDomain.BaseDirectory);
        }
        public static string convertInvalidFileName(string strFileName,string replaceChar="")
        {
            StringBuilder rBuilder = new StringBuilder(strFileName);
            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                rBuilder.Replace(rInvalidChar.ToString(), replaceChar);
            }
            return rBuilder.ToString();
        }
        public static string convertInvalidFolderName(string rPath, string replaceChar = "")
        {
            StringBuilder rBuilder = new StringBuilder(rPath);
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
            {
                rBuilder.Replace(rInvalidChar.ToString(), replaceChar);
            }
            return rBuilder.ToString();
        }
    }
}
