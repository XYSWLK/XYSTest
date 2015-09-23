using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYS.DAL.Lis;
using XYS.Model;
using XYS.Utility.DB;
using XYS.Lis.Model;

namespace XYS.Lis.DAL
{
    public class ReportItemDAL:IReportItemDAL<ReportItemModel>
    {
        #region 公共方法
       
        //获取单条记录
        public ReportItemModel Search(Hashtable equalFields)
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
                ReportItemModel rim;
                foreach (DataRow dr in dt.Rows)
                {
                    rim = new ReportItemModel();
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
                ReportItemModel rim;
                foreach (DataRow dr in dt.Rows)
                {
                    rim = new ReportItemModel();
                    FillData(rim, dr);
                    rimList.Add(rim);
                }
            }
        }
        //获取单条记录
        private ReportItemModel Query(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                ReportItemModel rim = new ReportItemModel();
                FillData(rim, dt.Rows[0]);
                return rim;
            }
            else
            {
                return null;
            }
        }
        //获得sql语句
        private string GenderSQL(Hashtable equalFields)
        {
            string sql = "select receivedate,sectionno,testtypeno,sampleno,t.CName as testitemname,t.EName as itemename,ISNULL(r.ReportDesc,'') + ISNULL(CONVERT(VARCHAR(50),r.ReportValue),'') as reportvalueall,resultstatus,r.unit as unit1,t.unit as unit2,refrange,disporder,r.itemno as itemno,secretgrade,paritemno,t.prec as precision2 from reportitem as r left outer join testitem as t on r.itemno=t.itemno";
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
        private void FillData(ReportItemModel rim,DataRow dr)
        {
            //rim.ReceiveDate = (DateTime)dr["receivedate"];
            //rim.SectionNo = (int)dr["sectionno"];
            //rim.TestTypeNo = (int)dr["testtypeno"];
            //rim.SampleNo = dr["sampleno"].ToString();
            rim.ItemNo = Convert.ToInt32(dr["itemno"]);
            rim.ItemName = dr["testitemname"] == DBNull.Value ? "" : dr["testitemname"].ToString();
            rim.ItemEName=dr["itemename"]==DBNull.Value?"":dr["itemename"].ToString();
            rim.ItemResult = dr["reportvalueall"] == DBNull.Value ? "" : dr["reportvalueall"].ToString();
            rim.ResultStatus = dr["resultstatus"] == DBNull.Value ? "" : dr["resultstatus"].ToString();
            if (dr["unit1"] != DBNull.Value && !dr["unit1"].ToString().Equals(""))
            {
                rim.ItemUnit = dr["unit1"].ToString();
            }
            else
            {
                rim.ItemUnit = dr["unit2"] == DBNull.Value ? "" : dr["unit2"].ToString();
            }
            rim.RefRange = dr["refrange"] == DBNull.Value ? "" : dr["refrange"].ToString();
            rim.DispNo=(int)dr["disporder"];
            rim.Precision = (int)dr["precision2"];
            rim.SecretGrade = (int)dr["secretgrade"];
            rim.ParItemNo = (int)dr["paritemno"];
        }
        
        #endregion
    }
}
