using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using XYS.Utility.DB;
namespace XYS.Lis.Utility
{
    public class PUser
    {
        private static readonly Hashtable UserTable = new Hashtable();
        public static byte[] GetUserImage(string userName)
        {
            if (UserTable.Count == 0)
            {
                //初始化
                InitUserTable();
            }
            return (byte[])UserTable[userName];
        }
        private static void InitUserTable()
        {

            string sql = "select cname,userimage from PUser where userimage is not null";
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            UserTable.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                UserTable.Add(dr["cname"].ToString(), (byte[])dr["userimage"]);
            }
        }
    }
}
