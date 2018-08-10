using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Gat.Controls;
using System.Collections.ObjectModel;
using AppsExplorer.CustomFunction;
using System.IO;
using System.Diagnostics;
using Gat.Controls.Model;
using AppsExplorer.CustomFunction.GatControlEx;
using System.Threading;


namespace AppsExplorer
{
    /// <summary>
    /// Interaction logic for addAppItem.xaml
    /// </summary>
    public partial class makeAppZip : MetroWindow
    {
        public makeAppZip()
        {
            InitializeComponent();
        }

        private void makeAppZip_Click(object sender, RoutedEventArgs e)
        {
            #region 检查输入是否正确
            MessageDialogResult result;
            string filepath =folderName.Text;
            string filename = fileName.Text;
            if (filepath.Length <= 0||filepath.Length <= 0)
            {
                result = this.ShowModalMessageExternal("Warning", "Please select a local resource.");
                if (result == MessageDialogResult.Affirmative)
                {
                    step1.IsSelected = true;
                    UpdateLayout();
                    selectAppResource.Focus();
                    return;
                }
            }
            appName.Text = pathConverter.convertInvalidFolderName(appName.Text, "");
            string appname = appName.Text;
            if (appname.Length <= 0)
            {
                result = this.ShowModalMessageExternal("Warning", "Please set pre-defined APP Name.");
                if (result == MessageDialogResult.Affirmative)
                {
                    step2.IsSelected = true;
                    UpdateLayout();
                    appName.Focus();
                    return;
                }
            }
            string appdescription = appDescription.Text;
            if (appdescription.Length <= 0)
            {
                result = this.ShowModalMessageExternal("Warning", "Please set pre-defined APP Description.");
                if (result == MessageDialogResult.Affirmative)
                {
                    step2.IsSelected = true;
                    UpdateLayout();
                    appDescription.Focus();
                    return;
                }
            }
            string zipname = appzipName.Text;
            if (zipname.Length <= 0)
            {
                result = this.ShowModalMessageExternal("Warning", "Please select a target path for location of .appZip package.");
                if (result == MessageDialogResult.Affirmative)
                {
                    step2.IsSelected = true;
                    UpdateLayout();
                    saveAsAppZipPath.Focus();
                    return;
                }
            }
            else
            {
                if (File.Exists(zipname))
                {
                    try
                    {
                        File.Delete(zipname);
                    }
                    catch(Exception ex)
                    {

                        result = this.ShowModalMessageExternal("Error", "Failed to auto delete old file:\n"+ zipname + "\n"+ex.Message);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            return;
                        }
                    }

                }
            }
            #endregion 
            string log = System.IO.Path.Combine(Directory.GetParent(filepath).FullName, "AppZipPackage_"+Guid.NewGuid().ToString("B")+".log");
            processBar.showRingBar(this, () => {
                try
                {
                    FileStream fs = new FileStream(log, FileMode.OpenOrCreate);
                    new FileInfo(log).Attributes = FileAttributes.Hidden;
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(new string('-', 150));
                    sw.WriteLine("Logs for App Package:"+ zipname);
                    sw.WriteLine("Created on time: "+DateTime.Now.ToString("G"));
                    sw.WriteLine("Created by: " + Environment.UserName+"/"+Environment.UserDomainName);
                    sw.WriteLine("Created from computer: " + Environment.MachineName);
                    sw.WriteLine("Created from original path: " + filepath);
                    sw.WriteLine("Generated to: " + zipname);
                    sw.WriteLine(new string('-', 150));
                    sw.WriteLine("APP Information is Shown As Below:");
                    sw.WriteLine("APP Name:"+appname);
                    sw.WriteLine("APP DesCription:" + appdescription);
                    string relativePath = pathConverter.getRelativePath(filename, filepath);
                    sw.WriteLine("App Relative Path:"+ relativePath);
                    sw.WriteLine(new string('-', 150));
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    //开始压缩
                    List<string> list = new List<string>();
                    list.Add(filepath);
                    list.Add(log);
                    sharpZipHelper.CompressFile(list, zipname);
                    File.Delete(log);
                    
                }
            
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke((Action)delegate
                    {
                        throw ex;
                    });
                }

            }, "Waiting", "Creating new App Zip package...");
            this.Close();
        }

        private void selectAppResource_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogView openFolderDialog = new OpenDialogView();
            OpenDialogViewModel folderDialogView = (OpenDialogViewModel)openFolderDialog.DataContext;
            folderDialogView.Caption = "Select a Resource Folder";
            folderDialogView.OpenText = "Select";
            folderDialogView.FileNameText = "Folder:";
            folderDialogView.FileFilterText = "Local:";
            folderDialogView.IsDirectoryChooser = true;
            if (folderDialogView.Show() == true)
            {
                string path = folderDialogView.SelectedFilePath;
                if (path!=null)
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        OpenDialogView openDialog = new OpenDialogView();
                        OpenDialogViewModel dialogView = (OpenDialogViewModel)openDialog.DataContext;
                        #region 选择为前一步所选的文件夹
                        string showName;
                        if (System.IO.Path.GetPathRoot(path) == path)
                        {
                            showName = path;
                        }
                        else
                        {
                            showName = path.Substring(path.LastIndexOf(@"\") + 1);
                        }
                        dialogView.Items.Clear();
                        OpenDialogViewModelEx.addPathOpenDialog(dialogView, path, showName, "\\Icons\\folder.ico", "\\Icons\\folder.ico", true);
                        dialogView.Caption = "Select a Executable File From Resource Folder";
                        dialogView.OpenText = "Select";
                        #endregion
                        if (dialogView.Show() == true)
                        {
                            string file = dialogView.SelectedFilePath;
                            if (file != null)
                            {
                                if (System.IO.File.Exists(file))
                                {
                                    folderName.Text = path;
                                    fileName.Text = file;
                                }
                            }
                        }
                    }

                }
            }
        }


        private void closeForm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveAsAppZipPath_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogView openFolderDialog = new OpenDialogView();
            OpenDialogViewModel folderDialogView = (OpenDialogViewModel)openFolderDialog.DataContext;
            folderDialogView.Caption = "Select Path for Saving .appZip Package";
            folderDialogView.SaveText = "Save";
            folderDialogView.FileNameText = "Name(.appZip):";
            folderDialogView.FileFilterText = "Type(.appZip):";
            folderDialogView.FileFilterExtensions.Clear();
            folderDialogView.FileFilterExtensions.Add(".appZip");
            folderDialogView.SelectedFileFilterExtension = ".appZip";
            folderDialogView.IsSaveDialog=true;
            OpenDialogViewModelEx.addDesktop(folderDialogView);
            if (folderDialogView.Show() == true)
            {
                if (folderDialogView.SelectedFolder is object)
                {
                    string path =pathConverter.convertInvalidFolderName( folderDialogView.SelectedFolder.Path);
                    string name = pathConverter.convertInvalidFileName(folderDialogView.SelectedFilePath == null ? string.Empty : folderDialogView.SelectedFilePath).TrimStart();
                    if (System.IO.Directory.Exists(path) && name.Length > 0)
                    {
                        appzipName.Text = System.IO.Path.Combine(path, name + ".appZip");
                    }
                }

            }
        }
    }
}
