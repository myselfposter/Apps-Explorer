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
using System.Threading;
using System.ComponentModel;

namespace AppsExplorer
{
    /// <summary>
    /// Interaction logic for processBar.xaml
    /// </summary>
    public partial class processBar : MetroWindow
    {
        private Action doAction;
        private processBar(string mainTitle,string subTitle,Action callback)
        {
            InitializeComponent();
            infoMain.Text = mainTitle;
            infoSub.Text = subTitle;
            this.doAction = callback;
        }

        private void action_Loaded(object sender, RoutedEventArgs e)
        {
            this.doAction.BeginInvoke(this.onDoingAction, null);
        }
        private void onDoingAction(IAsyncResult ar)
        {
            this.Dispatcher.Invoke(new Action(() => { this.Close(); }));
        }

        public static void showRingBar(FrameworkElement owner, Action callback, string maintitle, string subtitle)
        {
            processBar win = new processBar(maintitle, subtitle, callback);
            Window pwin = Window.GetWindow(owner);
            win.Owner = pwin;
            var loc = owner.PointToScreen(new Point());
            win.Left = loc.X + (owner.ActualWidth - win.Width) / 2;
            win.Top = loc.Y + (owner.ActualHeight - win.Height) / 2;
            win.ShowDialog();
        }

    }



}
