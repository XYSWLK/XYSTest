using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XYS.Lis.Model;
using XYS.Utility.DB;

namespace XYS.Lis.DAL
{
   public class GSReportItemDAL
    {
        #region 公共方法

        //获取单条记录
        public GSReportItemModel Search(Hashtable equalFields)
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
                GSReportItemModel rim;
                foreach (DataRow dr in dt.Rows)
                {
                    rim = new GSReportItemModel();
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
                GSReportItemModel gsrim;
                foreach (DataRow dr in dt.Rows)
                {
                    gsrim = new GSReportItemModel();
                    FillData(gsrim, dr);
                    rimList.Add(gsrim);
                }
            }
        }
        //获取单条记录
        private GSReportItemModel Query(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                GSReportItemModel gsrim = new GSReportItemModel();
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
            string sql = "select itemno,ISNULL(ReportDesc,'') + ISNULL(CONVERT(VARCHAR(50),ReportValue),'') as reportvalueall,reporttext,isfile,graphfilename from S_RequestVItem";
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
        private void FillData(GSReportItemModel gsrim, DataRow dr)
        {
            //rim.ReceiveDate = (DateTime)dr["receivedate"];
            //rim.SectionNo = (int)dr["sectionno"];
            //rim.TestTypeNo = (int)dr["testtypeno"];
            //rim.SampleNo = dr["sampleno"].ToString();
            gsrim.ItemNo = Convert.ToInt32(dr["itemno"]);
            gsrim.ItemResult = dr["reportvalueall"] == DBNull.Value ? "" : dr["reportvalueall"].ToString().Trim();
            gsrim.ReportText = dr["reporttext"] == DBNull.Value ? "" : dr["reporttext"].ToString().Trim();
            gsrim.IsFile = dr["isfile"] == DBNull.Value ? -1 : (int)dr["isfile"];
            gsrim.FilePath = dr["graphfilename"] == DBNull.Value ? "" : dr["graphfilename"].ToString().Trim();
        }

        #endregion
    }
}
