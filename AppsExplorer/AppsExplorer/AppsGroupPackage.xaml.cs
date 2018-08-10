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
using AppsExplorer.CustomFunction.GatControlEx;
using AppsExplorer.CustomFunction;
using System.IO;

namespace AppsExplorer
{
    /// <summary>
    /// Interaction logic for AppsGroupPackage.xaml
    /// </summary>
    public partial class AppsGroupPackage : MetroWindow
    {
        public AppsGroupPackage( int i)
        {
            InitializeComponent();
            if(i<=0)
            {
                tabmain.SelectedIndex = 0;//save
            }
            else
            {
                tabmain.SelectedIndex = 1;//load
            }
        }

        private void saveAsAppZipPackagePath_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogView openFolderDialog = new OpenDialogView();
            OpenDialogViewModel folderDialogView = (OpenDialogViewModel)openFolderDialog.DataContext;
            folderDialogView.Caption = "Select Path for Saving .appPckZip Package";
            folderDialogView.SaveText = "Save";
            folderDialogView.FileNameText = "Name(.appPckZip):";
            folderDialogView.FileFilterText = "Type(.appPckZip):";
            folderDialogView.FileFilterExtensions.Clear();
            folderDialogView.FileFilterExtensions.Add(".appPckZip");
            folderDialogView.SelectedFileFilterExtension = ".appPckZip";
            folderDialogView.IsSaveDialog = true;
            OpenDialogViewModelEx.addDesktop(folderDialogView);
            if (folderDialogView.Show() == true)
            {
                if (folderDialogView.SelectedFolder is object)
                {
                    string path = pathConverter.convertInvalidFolderName(folderDialogView.SelectedFolder.Path);
                    string name = pathConverter.convertInvalidFileName(folderDialogView.SelectedFilePath == null ? string.Empty : folderDialogView.SelectedFilePath).TrimStart();
                    if (System.IO.Directory.Exists(path) && name.Length > 0)
                    {
                        saveZipName.Text = System.IO.Path.Combine(path, name + ".appPckZip");
                    }
                }

            }
        }


        private void saveAppsGroup_Click(object sender, RoutedEventArgs e)
        {
            #region 检查保存文件配置，如果目标文件存在则尝试删除
            string zipname = saveZipName.Text;
            if (zipname.Length <= 0)
            {
                var result = this.ShowModalMessageExternal("Warning", "Please select a target path for location of .appPckZip package.");
                if (result == MessageDialogResult.Affirmative)
                {
                    UpdateLayout();
                    saveZipName.Focus();
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
                    catch (Exception ex)
                    {

                        var result = this.ShowModalMessageExternal("Error", "Failed to auto delete old file:\n" + zipname + "\n" + ex.Message);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            return;
                        }
                    }

                }
            }
            #endregion
            
            processBar.showRingBar(this, () =>
            {
                try
                {
                    
                    //开始压缩
                    List<string> list = new List<string>();
                    list.Add(System.AppDomain.CurrentDomain.BaseDirectory + "AppsData");
                    sharpZipHelper.CompressFile(list, zipname);

                }

                catch (Exception ex)
                {
                    this.Dispatcher.Invoke((Action)delegate
                    {
                        throw ex;
                    });
                }

            }, "Waiting", "Creating new AppsGroup Zip package...");
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loadAppZipPackagePath_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogView openDialog = new OpenDialogView();
            OpenDialogViewModel dialogView = (OpenDialogViewModel)openDialog.DataContext;
            OpenDialogViewModelEx.addDesktop(dialogView);
            dialogView.Caption = "Select a .appPckZip Package";
            dialogView.FileFilterExtensions.Clear();
            dialogView.FileFilterExtensions.Add(".appPckZip");
            dialogView.SelectedFileFilterExtension = ".appPckZip";
            if (dialogView.Show() == true)
            {
                string path = dialogView.SelectedFilePath;
                if (path != null)
                {
                    if (System.IO.File.Exists(path))
                    {
                        loadZipName.Text = path;
                    }
                }
            }
        }

        private void loadAppsGroup_Click(object sender, RoutedEventArgs e)
        {
            string zipName = loadZipName.Text;
            MessageDialogResult result;
            if (zipName.Length<=0)
            {
                result = this.ShowModalMessageExternal("Warning", "Please select a target path for location of .appPckZip package.");
                if (result == MessageDialogResult.Affirmative)
                {
                    UpdateLayout();
                    loadZipName.Focus();
                    return;
                }
            }
            if (!File.Exists(zipName))
            {
                result = this.ShowModalMessageExternal("Warning", "*.appPckZip package is missing:\n"+zipName);
                if (result == MessageDialogResult.Affirmative)
                {
                    UpdateLayout();
                    loadZipName.Text = "";
                    loadZipName.Focus();
                    return;
                }
            }
            result = this.ShowModalMessageExternal("Warning", "Before loading package, please be sure no APP is running!\nGO?",MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {


                processBar.showRingBar(this, () =>
                {
                    try
                    {
                        Directory.Delete(pathConverter.getAbsolutePathDefault("AppsData"), true);
                        //开始解压
                        sharpZipHelper.DecomparessFile(zipName, System.AppDomain.CurrentDomain.BaseDirectory);

                    }

                    catch (Exception ex)
                    {
                        this.Dispatcher.Invoke((Action)delegate
                        {
                            throw ex;
                        });
                    }

                }, "Waiting", "Loading AppsGroup Zip package...");
                
            }


        }
    }
}
