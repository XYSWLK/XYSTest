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
    public class RFGraphDataDAL:IRFGraphDataDAL<RFGraphDataModel>
    {
        public RFGraphDataModel Search(Hashtable equalFields)
        {
            return Query(equalFields);
        }
        public List<RFGraphDataModel> SearchList(Hashtable equalFields)
        {
            return QueryList(equalFields);
        }
        public Hashtable SearchTable(Hashtable equalFields)
        {
            return QueryTable(equalFields);
        }
        private List<RFGraphDataModel> QueryList(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                List<RFGraphDataModel> rfList = new List<RFGraphDataModel>();
                RFGraphDataModel rf;
                foreach (DataRow dr in dt.Rows)
                {
                    rf = new RFGraphDataModel();
                    FillData(rf, dr);
                    rfList.Add(rf);
                }
                return rfList;
            }
            else
            {
                return null;
            }
        }
        private RFGraphDataModel Query(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                RFGraphDataModel rf = new RFGraphDataModel();
                FillData(rf, dt.Rows[0]);
                return rf;
            }
            else
            {
                return null;
            }
        }
        private Hashtable QueryTable(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                FillData(equalFields, dt);
            }
            return equalFields;
        }
         private string GenderSQL(Hashtable equalFields)
        {
            string sql = " select graphname,graphjpg from RFGraphData";
            return sql + GetSQLWhere(equalFields);
        }
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
        private void FillData(RFGraphDataModel rim, DataRow dr)
        { 

        }
        private void FillData(Hashtable ht,DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ht.Remove(dr["graphname"].ToString().Trim());
                ht.Add(dr["graphname"].ToString().Trim(),(byte[])dr["graphjpg"]);
            }
        }
    }
}
