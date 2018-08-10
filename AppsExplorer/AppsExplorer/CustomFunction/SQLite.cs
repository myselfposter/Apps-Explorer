using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace AppsExplorer
{
    static class SQLite
    {
        
        static private SQLiteConnection connSQLite(string path)
        {
            //创建一个SQLite连接
            return new SQLiteConnection("Data Source="+path);
        }
        static public DataTable fillDataTablefromSQLite(string path,string sqlstring)
        {
            //使用指定的SQLite数据库和查询语句，返回到DataTable中
            SQLiteConnection conn = connSQLite(path);
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SQLiteDataAdapter sda = new SQLiteDataAdapter(sqlstring, conn);
                sda.Fill(dt);
                return dt;
            }
            finally
            {
                conn.Close();
            }
        }
        public static void clearTableContents(string[] tblList, string SQLitePath)
        {
            //清空表的数据，但不删除表
            SQLiteConnection conn = connSQLite(SQLitePath);
            try
            {
                conn.Open();
                if (tblList != null && tblList.Length > 0)
                {
                    for (int i = 0; i < tblList.Length; i++)
                    {
                        SQLiteCommand cmd = new SQLiteCommand("DELETE From " + tblList[i], conn);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            finally
            {
                conn.Close();
            }
        }
        public static void runSQLCmds(List<string> sqlCmd, string SQLiteConnPath)
        {
            SQLiteConnection conn = connSQLite(SQLiteConnPath);
            try
            {
                conn.Open();
                if (sqlCmd != null && sqlCmd.Count > 0)
                {
                    for (int i = 0; i < sqlCmd.Count; i++)
                    {
                        SQLiteCommand cmd = new SQLiteCommand(sqlCmd[i], conn);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            finally
            {
                conn.Close();
            }
        }
    }
}
