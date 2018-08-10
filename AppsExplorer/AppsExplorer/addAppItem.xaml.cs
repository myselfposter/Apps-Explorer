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
using AppsExplorer.CustomFunction.GatControlEx;


namespace AppsExplorer
{
    /// <summary>
    /// Interaction logic for addAppItem.xaml
    /// </summary>
    public partial class addAppItem : MetroWindow
    {
        private ObservableCollection<AppItem> appitems;
        public addAppItem(ObservableCollection<AppItem> appItemsList)
        {
            InitializeComponent();
            appitems = appItemsList;
        }

        private void addAPPItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialogResult result;
            string filepath = fileName.Text;
            if(filepath.Length<=0)
            {
                result= this.ShowModalMessageExternal("Warning", "Please select APP File.");
                if(result==MessageDialogResult.Affirmative)
                {
                    step1.IsSelected = true;
                    UpdateLayout();
                    selectAppFile.Focus();
                    return;
                }
            }
            string appname = appName.Text;
            if (appname.Length <= 0)
            {
                result = this.ShowModalMessageExternal("Warning", "Please set APP Name.");
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
                result = this.ShowModalMessageExternal("Warning", "Please set APP Description.");
                if (result == MessageDialogResult.Affirmative)
                {
                    step2.IsSelected = true;
                    UpdateLayout();
                    appDescription.Focus();
                    return;
                }
            }
            appitems.Add(new AppItem
            {
                AppDescription = appdescription,
                AppName = appname,
                AppPath = filepath,
                AppGUID = new Guid().ToString("B")
            });
            this.Close();
        }

        private void selectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenDialogView openDialog = new OpenDialogView();
            OpenDialogViewModel dialogView = (OpenDialogViewModel)openDialog.DataContext;
            OpenDialogViewModelEx.addDesktop(dialogView);
            dialogView.Caption = "Select a File";
            dialogView.OpenText = "Select";
            if (dialogView.Show() == true)
            {
                string path = dialogView.SelectedFilePath;
                if (path != null)
                {
                    if (System.IO.File.Exists(path))
                    {
                        fileName.Text = path;
                    }

                }
            }
        }

        private void closeForm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
