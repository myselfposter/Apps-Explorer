using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GongSolutions.Wpf.DragDrop;
using System.Data;
using System.Diagnostics;

namespace AppsExplorer
{
    public class appsModel
    {
        //用来绑定tabcontrol的模型组件
        private ObservableCollection<AppGroup> appGroups=new ObservableCollection<AppGroup>();
        public appsModel()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "AppsData\\AppsData.db3";
            DataTable dt = SQLite.fillDataTablefromSQLite(path, "SELECT * FROM tblAppGroup ORDER BY AppGroupSequence");
            appGroups.Clear();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region 从SQLite数据库的数据表tblAppGroup数据提取到dt，构建app模型以供tabcontrol使用
                string appgroupID = dt.Rows[i]["AppGroupGUID"].ToString();//功能组APPSGroupID，用来筛选子APP(GUID)
                ObservableCollection<AppItem> appitems = new ObservableCollection<AppItem>();
                DataTable dtapp = new DataTable();
                dtapp = SQLite.fillDataTablefromSQLite(path, "SELECT * FROM tblAppItem WHERE AppGroupID='"+appgroupID+"' ORDER BY AppSequence");
                for (int j = 0; j < dtapp.Rows.Count; j++)
                {
                    appitems.Add(new AppItem
                    {
                        ID = int.Parse(dtapp.Rows[j]["ID"].ToString()),
                        AppDescription = dtapp.Rows[j]["AppDescription"].ToString(),
                        AppGroupID = appgroupID,
                        AppSequence = int.Parse(dtapp.Rows[j]["AppSequence"].ToString()),
                        AppName = dtapp.Rows[j]["AppName"].ToString(),
                        AppPath = dtapp.Rows[j]["AppPath"].ToString(),
                        AppZipPath = dtapp.Rows[j]["AppZIPPath"].ToString(),
                        AppGUID= dtapp.Rows[j]["AppGUID"].ToString(),
                    });
                }

                appGroups.Add(new AppGroup()
                {
                    ID = int.Parse(dt.Rows[i]["ID"].ToString()),
                    AppGroupName = dt.Rows[i]["AppGroupName"].ToString(),
                    AppGroupGUID = appgroupID,
                    AppGroupSequence = int.Parse(dt.Rows[i]["AppGroupSequence"].ToString()),
                    appItems = appitems

                });
                #endregion
            }
        }
        public ObservableCollection<AppGroup> AppGroups {
            get
            {
                return appGroups;
            }
            private set
            {
                
            }
        }
    }
    
    //类：app组，对应tablcontrol页标题tablitem
    public class AppGroup: NotifyObject
    {
        public AppGroup()
        {
            appItems =  new ObservableCollection<AppItem>();
        }
        
        private int id;
        private string appGroupName;
        private int appGroupSequence;
        private string appGroupGUID;
        //每个app组包含多个app
        public ObservableCollection<AppItem> appItems { get; set; }
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                UpdateProperty(ref id, value);
            }
        }
        public string AppGroupName
        {
            get
            {
                return appGroupName;
            }
            set
            {
                UpdateProperty(ref appGroupName, value);
            }
        }
        public int AppGroupSequence
        {
            get
            {
                return appGroupSequence;
            }
            set
            {
                UpdateProperty(ref appGroupSequence, value);
            }
        }
        public string AppGroupGUID
        {
            get
            {
                return appGroupGUID;
            }
            set
            {
                UpdateProperty(ref appGroupGUID, value);
            }
        }

    }

    //类：app，具体某一个功能实现（归属于某个app组）
    public class AppItem : NotifyObject
    {
        private int id;
        private string appName;
        private int appSequence;
        private string appDescription;
        private string appPath;
        private string appZipPath;
        private string appGroupID;
        private string appGUID;
        private Process process;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                UpdateProperty(ref id, value);
            }
        }
        public string AppGroupID
        {
            get
            {
                return appGroupID;
            }
            set
            {
                UpdateProperty(ref appGroupID, value);
            }
        }
        public string AppName
        {
            get
            {
                return appName;
            }
            set
            {
                UpdateProperty(ref appName, value);
            }
        }
        public int AppSequence
        {
            get
            {
                return appSequence;
            }
            set
            {
                UpdateProperty(ref appSequence, value);
            }
        }
        public string AppDescription
        {
            get
            {
                return appDescription;
            }
            set
            {
                UpdateProperty(ref appDescription, value);
            }
        }
        public string AppPath
        {
            get
            {
                return appPath;
            }
            set
            {
                UpdateProperty(ref appPath, value);
            }
        }
        public string AppZipPath
        {
            get
            {
                return appZipPath;
            }
            set
            {
                UpdateProperty(ref appZipPath, value);
            }
        }
        public string AppGUID
        {
            get
            {
                return appGUID;
            }
            set
            {
                UpdateProperty(ref appGUID, value);
            }
        }
        public Process ProcessID
        {
            get
            {
                return process;
            }
            set
            {
                UpdateProperty(ref process, value);
            }
        }
    }
}
