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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.IO;
using AppsExplorer.CustomFunction;
using System.Net;
using Gat.Controls;
using System.Data;
using System.Diagnostics;
using AppsExplorer.CustomFunction.GatControlEx;
using System.Text.RegularExpressions;
using System.Threading;
using System.ComponentModel;

namespace AppsExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ObservableCollection<AppGroup> getAppsModel = new ObservableCollection<AppGroup>();
        public MainWindow()
        {
            InitializeComponent();
            #region tablcontrol绑定工作初始化
            getAppsModel = new appsModel().AppGroups;
            this.DataContext = getAppsModel;
            //绑定设置好的App清单
            appTabControl.SetBinding(MetroTabControl.ItemsSourceProperty, new Binding(".")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            });
            #endregion
            #region 底部状态栏初始化
            showDate.Text = "Date:   "+DateTime.Now.ToString("yyyy - MM - dd");
            showUser.Text = "User:   " + Environment.UserName;
            showUserDomain.Text = "Domain:   " + Environment.UserDomainName;
            showComputer.Text = "Computer Name:   " + Environment.MachineName;
            showIP.Text = "IP address:   " + GetLocalIp();
            #endregion
            
        }
        #region 获取当前IP地址
        private string GetLocalIp()
        {
            IPAddress localIp = null;

            try
            {
                IPAddress[] ipArray;
                ipArray = Dns.GetHostAddresses(Dns.GetHostName());
                localIp = ipArray.First(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            }
            catch
            {

            };
            if (localIp == null)
            {
                localIp = IPAddress.Parse("127.0.0.1");
            }
            return localIp.ToString();
        }
        #endregion

        private void MetroTabControl_TabItemClosingEvent(object sender, BaseMetroTabControl.TabItemClosingEventArgs e)
        {
            if (showDialogwhenDeleteAPPGroup.IsChecked == true)
            {
                String message = e.ClosingTabItem.Header.ToString();
                message = "Ready to remove Apps Group: " + message + " ?";
                message += "\nNote: Subordinate Apps will be removed also, be sure that all subordinate Apps is closed!";
                MetroDialogSettings mysetting = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Remove",
                    NegativeButtonText = "Cancel",
                    ColorScheme = MetroDialogOptions.ColorScheme,
                    DefaultButtonFocus = MessageDialogResult.Negative
                };
                MessageDialogResult result = this.ShowModalMessageExternal("Warning", message, MessageDialogStyle.AffirmativeAndNegative, mysetting);

                if (result == MessageDialogResult.Negative)
                {
                    e.Cancel = true;
                    return;
                }
            }
            //删除该AppGroup下的所有子项
            AppGroup appgroup = getAppsModel[appTabControl.SelectedIndex];
            string target = string.Empty;
            for (int i = appgroup.appItems.Count-1; i >=0; i--)
            {
                //对于直接运行程序的类型，删除记录即可；路径为绝对路径
                //对于从.appZip包解压过来的类型，还需要删除文件夹；路径为相对路径-appexplorer根目录\AppsData\+app Name
                target = appgroup.appItems[i].AppPath;
                if (!System.IO.Path.IsPathRooted(target))
                {
                    //.appZIP导过来的程序
                    try
                    {
                        target = pathConverter.getAbsolutePathDefault(appgroup.appItems[i].AppZipPath);//app文件夹位置
                        if (appgroup.appItems[i].ProcessID != null)
                        {
                            if (!appgroup.appItems[i].ProcessID.HasExited)
                            {
                                appgroup.appItems[i].ProcessID.Kill();
                                appgroup.appItems[i].ProcessID.WaitForExit();
                            }
                        }
                        if (Directory.Exists(target))
                        {
                            Directory.Delete(target, true);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        this.ShowModalMessageExternal("Error", "Error occurs while trying to remove APP:\n" + appgroup.appItems[i].AppName + "\nError Message: " + ex.Message, MessageDialogStyle.AffirmativeAndNegative);
                        e.Cancel = true;
                        return;
                    }
                }
                appgroup.appItems.RemoveAt(i);

            }
        }

        private void btnRunApp_Click(object sender, RoutedEventArgs e)
        {
            string guid = (sender as Control).Tag.ToString();
            AppGroup appgroup = getAppsModel[appTabControl.SelectedIndex];
            for (int i = 0; i < appgroup.appItems.Count; i++)
            {
                if (appgroup.appItems[i].AppGUID == guid)
                {
                    string target = appgroup.appItems[i].AppPath;
                    if(!System.IO.Path.IsPathRooted(target))
                    {
                        target =appgroup.appItems[i].AppZipPath + "\\" + appgroup.appItems[i].AppPath;
                        target = pathConverter.getAbsolutePathDefault(target);
                    }
                    
                    if (System.IO.Path.IsPathRooted(target))
                    {
                        if (File.Exists(target))
                        {
                            //进程为null说明还未开始运行；
                            //不为空：要区分是否进程已结束
                            if (appgroup.appItems[i].ProcessID == null)
                            {
                                appgroup.appItems[i].ProcessID = Process.Start(target);
                            }
                            else
                            {
                                if (appgroup.appItems[i].ProcessID.HasExited)
                                {
                                    appgroup.appItems[i].ProcessID = Process.Start(target);
                                }
                                else
                                {
                                    if (this.ShowModalMessageExternal("Note", "App: " + appgroup.appItems[i].AppName + " is already running.", MessageDialogStyle.Affirmative) == MessageDialogResult.Affirmative)
                                    {
                                        return;
                                    }
                                }

                            }
                        }
                        else
                        {
                            this.ShowModalMessageExternal("Error!", "App missing:\n" + target, MessageDialogStyle.Affirmative);
                        }
                    }
                }
            }

        }

        private void btnRemoveAPP_Click(object sender, RoutedEventArgs e)
        {
            if(showDialogwhenDeleteAPP.IsChecked==true)
            {
                MetroDialogSettings mysetting = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Remove",
                    NegativeButtonText = "Cancel",
                    ColorScheme = MetroDialogOptions.ColorScheme,
                    DefaultButtonFocus = MessageDialogResult.Negative
                };
                MessageDialogResult result = this.ShowModalMessageExternal("Warning", "Ready to remove App?", MessageDialogStyle.AffirmativeAndNegative, mysetting);

                if (result != MessageDialogResult.Affirmative)
                {
                    return;
                }
            }

            string guid = (sender as Control).Tag.ToString();
            AppGroup appgroup = getAppsModel[appTabControl.SelectedIndex];
            for (int i = 0; i < appgroup.appItems.Count; i++)
            {
                if (appgroup.appItems[i].AppGUID== guid)
                {
                    //对于直接运行程序的类型，删除记录即可；路径为绝对路径
                    //对于从.appZip包解压过来的类型，还需要删除文件夹；路径为相对路径-appexplorer根目录\AppsData\+app Name
                    string target = appgroup.appItems[i].AppPath;
                    if (!System.IO.Path.IsPathRooted(target))
                    {
                        //.appZIP导过来的程序
                        try
                        {
                            target = pathConverter.getAbsolutePathDefault(appgroup.appItems[i].AppZipPath);//app文件夹位置
                            if (appgroup.appItems[i].ProcessID != null)
                            {
                                if (!appgroup.appItems[i].ProcessID.HasExited)
                                {
                                    appgroup.appItems[i].ProcessID.Kill();
                                    appgroup.appItems[i].ProcessID.WaitForExit();
                                }
                            }
                            if(Directory.Exists(target))
                            {
                                Directory.Delete(target, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ShowModalMessageExternal("Error", "Error occurs while trying to remove APP:\n"+ appgroup.appItems[i].AppName+"\nError Message: "+ex.Message, MessageDialogStyle.AffirmativeAndNegative);
                            return;
                        }
                    }
                    //无论何种类型，都要删清单
                    appgroup.appItems.RemoveAt(i);
                    return;
                }
            }
        }

        private void LaunchAppsExplorerOnGitHub(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/myselfposter/Apps-Explorer");
        }



        private void AboutAppsExplorer_Click(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.ShowDialog();
        }

        private void HelpAppsExplorer_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddNewAppsGroup_Click(object sender, RoutedEventArgs e)
        {
            var result = this.ShowModalInputExternal("Define New AppsGroup", "Name of new AppsGroup:");
            if (result == null)
            {
                return;
            }
            AppGroup newItem = new AppGroup()
            {
                AppGroupName = string.Empty + result,
                appItems = new ObservableCollection<AppItem>()
            };

            getAppsModel.Add(newItem);
            appTabControl.SelectedIndex = appTabControl.Items.Count - 1;

        }

        private void MakeNewApp_Click(object sender, RoutedEventArgs e)
        {
            makeAppZip makeappZip = new makeAppZip();
            makeappZip.ShowDialog();
        }


        private void LoadFromappZip_Click(object sender, RoutedEventArgs e)
        {
            if (getAppsModel.Count <= 0)
            {
                var result = this.ShowModalInputExternal("Note", "Add at least one new AppsGroup first.");
                if (result == null)
                {
                    return;
                }
                AppGroup newItem = new AppGroup()
                {
                    AppGroupName =  result,
                    appItems = new ObservableCollection<AppItem>()
                };

                getAppsModel.Add(newItem);
                appTabControl.SelectedIndex = 0;
            }
            //选择.appZip包
            OpenDialogView openDialog = new OpenDialogView();
            OpenDialogViewModel dialogView = (OpenDialogViewModel)openDialog.DataContext;
            OpenDialogViewModelEx.addDesktop(dialogView);
            dialogView.Caption = "Select a .appZip Package";
            dialogView.FileFilterExtensions.Clear();
            dialogView.FileFilterExtensions.Add(".appZip");
            dialogView.SelectedFileFilterExtension = ".appZip";
            if (dialogView.Show() == true)
            {
                string path = dialogView.SelectedFilePath;
                if (path != null)
                {
                    if (System.IO.File.Exists(path))
                    {
                        //开始解压.appZip
                        //        ^AppZipPackage_{\w{8}-(\w{4}-){3}\w{12}}.log$           此为正则表达式匹配log日志

                        processBar.showRingBar(this, () => {
                            try
                            {
                                string guid = Guid.NewGuid().ToString("B");
                                string appMainFolder = System.AppDomain.CurrentDomain.BaseDirectory;
                                string logFile = string.Empty;
                                string appName = string.Empty;
                                string appDescription = string.Empty;
                                string appRelativePath = string.Empty;//可执行文件，相对路径相对appZipPath
                                string appZipPath = "APPsData\\" + guid;  //项目下文件夹，如果存在则新建GUID文件夹,作为APP的容器文件夹
                                string appAbsolutePath = pathConverter.getAbsolutePath(appZipPath, appMainFolder);
                                if (Directory.Exists(appAbsolutePath))
                                {
                                    do
                                    {
                                        guid = Guid.NewGuid().ToString("B");
                                        appZipPath = "APPsData\\" + guid;
                                        appAbsolutePath = pathConverter.getAbsolutePath(appZipPath, appMainFolder);
                                    }
                                    while (!Directory.Exists(appAbsolutePath));

                                }
                                sharpZipHelper.DecomparessFile(path, appAbsolutePath);

                                string[] searchLog = Directory.GetFiles(appAbsolutePath, @"*.log", SearchOption.TopDirectoryOnly);
                                if (searchLog.Length > 0)
                                {
                                    for (int i = 0; i < searchLog.Length; i++)
                                    {
                                        if (Regex.Match(System.IO.Path.GetFileName(searchLog[i]), @"^AppZipPackage_{\w{8}-(\w{4}-){3}\w{12}}.log$").Success)
                                        {
                                            logFile = searchLog[i];
                                            break;
                                        }
                                    }
                                    if (logFile.Length > 0)
                                    {
                                        //成功取得log文件
                                        using (StreamReader sr = new StreamReader(new FileStream(logFile, FileMode.Open, FileAccess.Read), Encoding.UTF8))
                                        {
                                            String line;
                                            while ((line = sr.ReadLine()) != null)
                                            {
                                                if (line.StartsWith("APP Name:"))
                                                {
                                                    appName = line.Substring(9);
                                                    continue;
                                                }
                                                if (line.StartsWith("APP DesCription:"))
                                                {
                                                    appDescription = line.Substring(16);
                                                    continue;
                                                }
                                                if (line.StartsWith("App Relative Path:"))
                                                {
                                                    appRelativePath = line.Substring(18);
                                                    continue;
                                                }
                                            }
                                        }
                                        if (appName.Length > 0 && appDescription.Length > 0 && appRelativePath.Length > 0)
                                        {
                                            //加入列表,调用主窗口线程
                                            this.Dispatcher.Invoke(
                                                new Action(
                                                    delegate
                                                    {
                                                        getAppsModel[appTabControl.SelectedIndex].appItems.Add(
                                                        new AppItem
                                                        {
                                                            AppName = appName,
                                                            AppDescription = appDescription,
                                                            AppGUID = Guid.NewGuid().ToString("B"),
                                                            AppZipPath = appZipPath,
                                                            AppPath = appRelativePath
                                                        });
                                                    }));


                                        }
                                        else
                                        {
                                            Directory.Delete(appAbsolutePath);
                                            this.ShowModalMessageExternal("Error", "Log file is bad for .appZip file.\nQuit!", MessageDialogStyle.Affirmative);
                                        }
                                    }
                                    else
                                    {
                                        Directory.Delete(appAbsolutePath);
                                        this.ShowModalMessageExternal("Error", "Log missing for .appZip file.\nQuit!", MessageDialogStyle.Affirmative);
                                    }
                                }
                                else
                                {
                                    Directory.Delete(appAbsolutePath, true);
                                    this.ShowModalMessageExternal("Error", "Log missing for .appZip file.\nQuit!", MessageDialogStyle.Affirmative);
                                }
                            }
                            catch(Exception ex)
                            {
                                this.Dispatcher.Invoke((Action)delegate
                                {
                                    throw ex;
                                });
                            }
                        }, "Waiting...", "Unziping package...");
                    }
                }
            }

        }


        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            if(getAppsModel.Count<=0)
            {
                var result = this.ShowModalInputExternal("Note", "Add at least one new AppsGroup firstly.");
                if (result == null)
                {
                    return;
                }
                AppGroup newItem = new AppGroup()
                {
                    AppGroupName = result,
                    appItems = new ObservableCollection<AppItem>()
                };

                getAppsModel.Add(newItem);
                appTabControl.SelectedIndex = 0;
                
            }
            addAppItem addappitem = new addAppItem(getAppsModel[appTabControl.SelectedIndex].appItems);
            addappitem.ShowDialog();

        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //自动保存功能,setting中设置
            Properties.Settings.Default.Save();
            foreach(AppGroup appgroup in getAppsModel)
            {
                foreach(AppItem item in appgroup.appItems)
                {
                    if (item.ProcessID != null)
                    {
                        if (!item.ProcessID.HasExited)
                        {
                            item.ProcessID.Kill();
                            item.ProcessID.WaitForExit();
                        }
                    }
                }

            }
            if(enabledClosedSaveCurrentLayout.IsChecked==true)
            {
                saveCurrentAppsLayout();
            }
        }
        private void saveCurrentAppsLayout()
        {
            //保存当前功能区配置，以便下次启动时恢复
            //窗体退出时[是/否]自动运行
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "AppsData\\AppsData.db3";
            string[] tbl = new string[2] { "tblAppGroup", "tblAppItem" };
            SQLite.clearTableContents(tbl, path);
            List<string> sqlList = new List<string>();
            string newAppGroupGUID=string.Empty;
            for (int i = 0; i < getAppsModel.Count; i++)
            {
                newAppGroupGUID = Guid.NewGuid().ToString("B");
                sqlList.Add("INSERT INTO tblAppGroup(AppGroupName, AppGroupSequence,AppGroupGUID) Values('" + getAppsModel[i].AppGroupName + "', " + (i + 1) +",'"+ newAppGroupGUID + "')");
                for (int j = 0; j < getAppsModel[i].appItems.Count; j++)
                {
                    sqlList.Add("INSERT INTO tblAppItem(AppName,AppDescription,AppPath,AppZIPPath,AppSequence,AppGroupID,AppGUID) Values('" + getAppsModel[i].appItems[j].AppName + "','" + getAppsModel[i].appItems[j].AppDescription + "','" + getAppsModel[i].appItems[j].AppPath + "','"+ getAppsModel[i].appItems[j].AppZipPath+"'," + (j + 1).ToString() + ",'" + newAppGroupGUID + "','"+ Guid.NewGuid().ToString("B")+"')");
                }
            }
            SQLite.runSQLCmds(sqlList, path);
        }

        private void saveCurrentAppsLayout_Click(object sender, RoutedEventArgs e)
        {
            saveCurrentAppsLayout();
        }

        private void LoadAppsGroupPackage_Click(object sender, RoutedEventArgs e)
        {
            AppsGroupPackage frm = new AppsGroupPackage(1);
            frm.Show();
            this.Close();
        }

        private void SaveAppsGroupPackage_Click(object sender, RoutedEventArgs e)
        {
            AppsGroupPackage frm = new AppsGroupPackage(0);
            frm.Show();
            this.Close();
        }

        private void exitForm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
