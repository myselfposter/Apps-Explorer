using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gat.Controls.Model;
using System.IO;

namespace AppsExplorer.CustomFunction.GatControlEx
{
    //实测：不要在IsDirectoryChooser中使用
    public static class OpenDialogViewModelEx
    {
        public static void addDesktop(Gat.Controls.OpenDialogViewModel dialogView)
        {
            ImageSource img = new BitmapImage(new Uri("\\Icons\\desktop.ico", UriKind.RelativeOrAbsolute));
            ImageSource imgsub = new BitmapImage(new Uri("\\Icons\\folder.ico", UriKind.RelativeOrAbsolute));
            addPathOpenDialog(dialogView, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DeskTop", img,imgsub,true);
        }

        public static void addPathOpenDialog(Gat.Controls.OpenDialogViewModel dialogView,string path,string showName, ImageSource img, ImageSource imgSubItem, bool showChilds =true)
        {
            #region 为对话框增加一个路径为Path的选择项,显示名称showName，主图标img；子图标imgSubItem,是否显示子项showChilds(默认显示)
            OpenFolderRoot folder = new OpenFolderRoot();
            folder.Image = img;
            folder.Name = showName;
            folder.Path = path;
            if(showChilds)
            {
                DirectoryInfo root = new DirectoryInfo(path);
                List<OpenFolderItem> openItems = new List<OpenFolderItem>();
                foreach (DirectoryInfo dir in root.GetDirectories())
                {
                    OpenFolderItem item = new OpenFolderItem(dir.FullName);
                    item.Name = dir.FullName.Substring(dir.FullName.LastIndexOf(@"\") + 1);
                    item.Image = imgSubItem;
                    openItems.Add(item);
                }
                folder.Children = (ICollection<OpenFolderItem>)openItems;
            }
            dialogView.Items.Add(folder);
            #endregion
        }
        public static void addPathOpenDialog(Gat.Controls.OpenDialogViewModel dialogView, string path, string showName, string imgPath, string imgSubItemPath, bool showChilds = true)
        {
            ImageSource img = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
            ImageSource imgsub = new BitmapImage(new Uri(imgSubItemPath, UriKind.RelativeOrAbsolute));
            addPathOpenDialog(dialogView, path, showName, img, imgsub, showChilds);
        }
        public static void addPathOpenDialog(Gat.Controls.OpenDialogViewModel dialogView, string path, string showName, ImageSource img, string imgSubItemPath, bool showChilds = true)
        {
            ImageSource imgsub = new BitmapImage(new Uri(imgSubItemPath, UriKind.RelativeOrAbsolute));
            addPathOpenDialog(dialogView, path, showName, img, imgsub, showChilds);
        }
        public static void addPathOpenDialog(Gat.Controls.OpenDialogViewModel dialogView, string path, string showName, string imgPath, ImageSource imgSubItem, bool showChilds = true)
        {
            ImageSource img = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
            addPathOpenDialog(dialogView, path, showName, img, imgSubItem, showChilds);
        }
    }
}
