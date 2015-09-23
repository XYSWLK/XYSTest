using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XYS.Lis.Model;
using XYS.Utility.DB;

namespace XYS.Lis.DAL
{
    public class GSCommonItemDAL
    {
        #region 公共方法

        //获取单条记录
        public GSCommonItemModel Search(Hashtable equalFields)
        {
            return Query(equalFields);
        }
        //获取多条记录
        public List<ReportItemModel> SearchList(Hashtable equalFields)
        {
            return QueryList(equalFields);
        }
        public void SearchList(Hashtable equalFields, List<ReportItemModel> rimList)
        {
            QueryList(equalFields, rimList);
        }

        #endregion
        #region 私有方法

        //获取多条记录
        private List<ReportItemModel> QueryList(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                List<ReportItemModel> rfList = new List<ReportItemModel>();
                GSCommonItemModel rim;
                foreach (DataRow dr in dt.Rows)
                {
                    rim = new GSCommonItemModel();
                    FillData(rim, dr);
                    rfList.Add(rim);
                }
                return rfList;
            }
            else
            {
                return null;
            }
        }
        private void QueryList(Hashtable equalFields, List<ReportItemModel> rimList)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                GSCommonItemModel gsrim;
                foreach (DataRow dr in dt.Rows)
                {
                    gsrim = new GSCommonItemModel();
                    FillData(gsrim, dr);
                    rimList.Add(gsrim);
                }
            }
        }
        //获取单条记录
        private GSCommonItemModel Query(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                GSCommonItemModel gsrim = new GSCommonItemModel();
                FillData(gsrim, dt.Rows[0]);
                return gsrim;
            }
            else
            {
                return null;
            }
        }
        //获得sql语句
        private string GenderSQL(Hashtable equalFields)
        {
            string sql = "select itemno,bloodpercent,marrowpercent from reportmarrow";
            return sql + GetSQLWhere(equalFields);
        }
        //获得where语句
        private string GetSQLWhere(Hashtable equalFields)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            foreach (DictionaryEntry de in equalFields)
            {
                //int
                if (de.Value.GetType().FullName == "System.Int32")
                {
                    sb.Append(de.Key);
                    sb.Append("=");
                    sb.Append(de.Value);
                }
                //datetime
                else if (de.Value.GetType().FullName == "System.DateTime")
                {
                    DateTime dt = (DateTime)de.Value;
                    sb.Append(de.Key);
                    sb.Append("='");
                    sb.Append(dt.Date.ToString("yyyy-MM-dd"));
                    sb.Append("'");
                }
                //其他类型
                else
                {
                    sb.Append(de.Key);
                    sb.Append("='");
                    sb.Append(de.Value.ToString());
                    sb.Append("'");
                }
                sb.Append(" and ");
            }
            sb.Remove(sb.Length - 5, 5);
            return sb.ToString();
        }
        //填充数据到模板
        private void FillData(GSCommonItemModel gsrim, DataRow dr)
        {
            gsrim.ItemNo = Convert.ToInt32(dr["itemno"]);
            gsrim.XueValue = dr["bloodpercent"] == DBNull.Value ? "" : dr["bloodpercent"].ToString();
            gsrim.GSValue = dr["marrowpercent"] == DBNull.Value ? "" : dr["marrowpercent"].ToString();
        }
        #endregion
    }
}
