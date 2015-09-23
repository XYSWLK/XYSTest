using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XYS.DAL.Lis;
using XYS.Model;
using XYS.Utility.DB;
using XYS.Lis.Model;
using XYS.Lis.Utility;
namespace XYS.Lis.DAL
{
    public class ReportFormDAL:IReportFormDAL<ReportFormModel>
    {
        #region 公共方法
        public ReportFormModel Search(Hashtable equalFields)
        {
            return Query(equalFields);
        }
        public void Search(Hashtable equalFields,ReportFormModel rfm)
        {
            Query(equalFields, rfm);
        }
        //获取多条reportfrom记录
        public List<ReportFormModel> SearchList(Hashtable equalFields)
        {
            return QueryList(equalFields);
        }

        #endregion
        #region 私有方法

        //获取单个记录
        private ReportFormModel Query(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                ReportFormModel rfm = new ReportFormModel();
                FillData(rfm, dt.Rows[0]);
                return rfm;
            }
            else
            {
                return null;
            }
        }
        private void Query(Hashtable equalFields,ReportFormModel rfm)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                FillData(rfm, dt.Rows[0]);
            }
            else
            {
                //
            }
        }
       //获取多天记录
        private List<ReportFormModel> QueryList(Hashtable equalFields)
        {
            string sql = GenderSQL(equalFields);
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            if (dt.Rows.Count > 0)
            {
                List<ReportFormModel> rfList=new List<ReportFormModel>();
                ReportFormModel rfm;
                foreach(DataRow dr in dt.Rows)
                {
                    rfm = new ReportFormModel();
                    FillData(rfm, dr);
                    rfList.Add(rfm);
                }
                return rfList;
            }
            else
            {
                return null;
            }
        }
        //获取sql 语句
        private string GenderSQL(Hashtable equalFields)
        {
            string sql = "select sectionno,serialno,deptname,bed,sampleno,cname,gendername,age,ageunitno,patno,sampletypename,sicktypename,doctorname,zdy2,zdy5,formdesc,collectdate,collecttime,inceptdate,incepttime,checkdate,checktime,testdate,hospitalizedtimes,id_number_patient,sicktypeno,sampletypeno,technician,checker,formmemo,formcomment,formcomment2,paritemname,testtypeno,receivedate,sendertime2 from reportformfull";
            return sql + GetSQLWhere(equalFields);
        }
        private string GetSQLWhere(Hashtable equalFields) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            foreach (DictionaryEntry de in equalFields)
            {
                //int
                if(de.Value.GetType().FullName=="System.Int32")
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
        private void FillData(ReportFormModel rfm,DataRow dr)
        {
            rfm.SerialNo = dr["serialno"] == DBNull.Value ? "" : dr["serialno"].ToString().Trim();
            rfm.DeptName = dr["deptname"] == DBNull.Value ? "" : dr["deptname"].ToString().Trim();
            rfm.BedNo = dr["bed"] == DBNull.Value ? "" : dr["bed"].ToString().Trim();
            rfm.CName = dr["cname"] == DBNull.Value ? "" : dr["cname"].ToString().Trim();
            rfm.Sex = dr["gendername"] == DBNull.Value ? "" : dr["gendername"].ToString().Trim();
            int ageValue = dr["age"] == DBNull.Value ? 1 : Convert.ToInt32(dr["age"]);
            int ageUnit = dr["ageunitno"] == DBNull.Value ? 1 : Convert.ToInt32(dr["ageunitno"]);
            rfm.Ager = new Age(ageValue, (AgeUnit)ageUnit);
            rfm.PID = dr["patno"] == DBNull.Value ? "" : dr["patno"].ToString().Trim();
            rfm.SampleTypeName = dr["sampletypename"] == DBNull.Value ? "" : dr["sampletypename"].ToString();
            rfm.ClinicTypeName = dr["sicktypename"] == DBNull.Value ? "" : dr["sicktypename"].ToString();
            rfm.BillingDoctor = dr["doctorname"] == DBNull.Value ? "" : dr["doctorname"].ToString();
            rfm.ClinicalDiagnosis = dr["zdy2"] == DBNull.Value ? "" : dr["zdy2"].ToString().Trim();
            rfm.ZDY5 = dr["zdy5"] == DBNull.Value ? "" : dr["zdy5"].ToString().Trim();
            rfm.Explanation = dr["formdesc"] == DBNull.Value ? "" : dr["formdesc"].ToString();
            //主键
            rfm.SampleNo = dr["sampleno"].ToString().Trim();
            rfm.SectionNo = Convert.ToInt32(dr["sectionno"]);
            rfm.TestTypeNo = Convert.ToInt32(dr["testtypeno"]);
            rfm.ReceiveDate = (DateTime)dr["receivedate"];
            //检验时间、检验组合
            rfm.TestDateTime = dr["testdate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["testdate"];
            rfm.ParItemName = dr["paritemname"] == DBNull.Value ? "" : dr["paritemname"].ToString();
            //采集时间
            DateTime collectDate = dr["collectdate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["collectdate"];
            DateTime collectTime = dr["collecttime"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["collecttime"];
            rfm.CollectDateTime = DateAddTime(collectDate, collectTime);
            // 签收时间
            DateTime inceptDate = dr["inceptdate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["inceptdate"];
            DateTime inceptTime = dr["incepttime"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["incepttime"];
            rfm.InceptDateTime = DateAddTime(inceptDate, inceptTime);
           //审核时间 若有二审则以二审时间为审核时间
            if (dr["sendertime2"] == DBNull.Value)
            {
                DateTime reportDate = dr["checkdate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["checkdate"];
                DateTime reportTime = dr["checktime"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["checktime"];
                rfm.ReportDateTime = DateAddTime(reportDate, reportTime);
            }
            else
            {
                rfm.ReportDateTime = (DateTime)dr["sendertime2"];
            }
            //结论、备注、解释等信息。
            rfm.Remark = "";
            rfm.FormMemo = dr["formmemo"] == DBNull.Value ? "" : dr["formmemo"].ToString();
            rfm.FormComment = dr["formcomment"] == DBNull.Value ? "" : dr["formcomment"].ToString();
            rfm.FormComment2 = dr["formcomment2"] == DBNull.Value ? "" : dr["formcomment2"].ToString();
            //住院次数，病人CID、门诊类型编号、样本类型编号
            rfm.VisitTimes = dr["hospitalizedtimes"] == DBNull.Value ? -1 : (int)dr["hospitalizedtimes"];
            rfm.CID = dr["id_number_patient"] == DBNull.Value ? "" : dr["id_number_patient"].ToString().Trim();
            rfm.SickTypeNo = dr["sicktypeno"] == DBNull.Value ? 0 : (int)dr["sicktypeno"];
            rfm.SampleTypeNo = dr["sampletypeno"] == DBNull.Value ? -1 : (int)dr["sampletypeno"];
            //检验者,审核者
            rfm.Technician = dr["technician"] == DBNull.Value ? "" : dr["technician"].ToString();
            rfm.Checker = dr["checker"] == DBNull.Value ? "" : dr["checker"].ToString();
            rfm.TechnicianImage = PUser.GetUserImage(rfm.Technician);
            rfm.CheckerImage = PUser.GetUserImage(rfm.Checker);

        }
        //其他辅助函数
        private DateTime DateAddTime(DateTime date,DateTime time)
        {
            date = date.AddHours(time.Hour);
            date = date.AddMinutes(time.Minute);
            date = date.AddSeconds(time.Second);
            date = date.AddMilliseconds(time.Millisecond);
            return date;
        }

        #endregion
    }
}
