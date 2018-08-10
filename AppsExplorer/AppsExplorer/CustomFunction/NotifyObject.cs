using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace AppsExplorer
{
    public class NotifyObject:INotifyPropertyChanged
        //WPF属性绑定通知
    {
        protected void UpdateProperty<T>(ref T propertyValue,T newValue,[CallerMemberName] string propertyName="")
        {
            if(object.Equals(propertyValue,newValue))
            {
                return;
            }
            propertyValue = newValue;
            OnPropertyChanged(propertyName);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
