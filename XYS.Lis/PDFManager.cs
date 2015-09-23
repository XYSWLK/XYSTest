using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using XYS.Lis.Utility;
using XYS.Lis.Model;
using XYS.Lis.DAL;
using XYS.Lis.Section;
using System.IO;
using System.Text;
using FastReport;
using FastReport.Export.Pdf;

using XYS.Lis.Enum;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace XYS.Lis
{
    public class PDFManager
    {
        #region 公共方法
        //
        public void PrintPDFReports(List<LisReport> reportList, string modelName)
        {
            DataSet ds = CreateDataSet("ReportTables.frd");
            FillDataSet(reportList, ds);
            Report report = new Report();
            report.Load(modelName);
            report.RegisterData(ds);
            report.Prepare();
            PDFExport export = new PDFExport();
            report.Export(export, "result.pdf");
            report.Dispose();
        }
        //生成pdf
        public DataSet GetPrintDataSet(string frdName)
        {
            string frdFullName = GetReportDataStructPath(frdName);
            DataSet ds = CreateDataSet(frdFullName);
            return ds;
        }
        public void GenderPDFReport(LisReport lr, string rootPath)
        {
            //DataSet ds = CreateDataSet("ReportTables.frd");
            DataSet ds = GetPrintDataSet("ReportTables.frd");
            FillDataSet(lr, ds);
            Report report = new Report();
            //获取模板
            string modelName = GetReportModelName(lr);
            //获取生成文件全路径
            string fileFullName = GenderFileFullName(lr.ReportInfo, rootPath, lr.OrderNo);
            report.Load(GetReprotModelPath(modelName));
            report.RegisterData(ds);
            report.Prepare();
            PDFExport export = new PDFExport();
            report.Export(export, fileFullName);
            report.Dispose();
        }
        public void GenderPDFReport(LisReport lr, string rootPath,DataSet ds)
        {
            //清空数据库
            ClearDataSet(ds);
            //填充数据
            FillDataSet(lr, ds);
            Report report = new Report();
            //获取模板
            string modelName = GetReportModelName(lr);
            //获取生成文件全路径
            string fileFullName = GenderFileFullName(lr.ReportInfo, rootPath, lr.OrderNo);
            report.Load(GetReprotModelPath(modelName));
            report.RegisterData(ds);
            report.Prepare();
            PDFExport export = new PDFExport();
            //这里针对像流式这种生成PDF之后还要处理的情况。
            if (AfterCreatPDF(lr.ReportInfo.SectionNo))
            {
                string linshi = rootPath + "\\liushi\\linshi.pdf";
                report.Export(export, linshi);
                report.Dispose();
                List<MyMergePdf> lm = new List<MyMergePdf>();
                lm.Add(new MyMergePdf { Name = linshi });
                string liushiPDFname = getliushiname(lr.ReportInfo.SerialNo, lr.ReportInfo.SampleNo, rootPath);
                if (liushiPDFname != "")
                {
                    try
                    {
                        int page = Convert.ToInt32(liushiPDFname.Split('_')[2].Split('.')[0]);
                        lm.Add(new MyMergePdf { Name = rootPath + "\\liushi\\" + liushiPDFname, PagCount = page });
                    }
                    catch { }
                }
                mergePDFFiles(lm, fileFullName);
            }
            //一般情况。
            else
            {
                report.Export(export, fileFullName);
                report.Dispose();
            }
        }
        public void PrintReport(LisReport lr)
        {
            DataSet ds = CreateDataSet("ReportTables.frd");
            FillDataSet(lr, ds);
            Report report = new Report();
            string modelName = GetReportModelName(lr);
            report.Load(GetReprotModelPath(modelName));
            report.RegisterData(ds);
            report.Prepare();
            report.Print();
        }
        
        #endregion

        #region 私有方法
        //根据xml生成dataset
        private DataSet CreateDataSet(string frdFile)
        {
            DataSet ds=XMLTools.ConvertFRDFile2DataSet(frdFile);
            return ds;
        }
        //将report集合填充到dataset
        private void FillDataSet(List<LisReport> reportList,DataSet ds)
        {
            foreach (LisReport lr in reportList)
            {
                FillDataSet(lr, ds);
            }
        }
        //将report对象填充到dataset
        private void FillDataSet(LisReport lr, DataSet ds)
        {
            FillReportInfo(lr.ReportInfo, ds);
            //特殊检验项填充
            switch (lr.ReportInfo.SectionNo)
            {
                //血细胞
                case 2:
                case 27:
                    //血常规
                    if (lr.SpecItemsTable.Count > 4)
                    {
                        //人工分类
                        FillManReportItem(lr.SpecItemsTable, ds);
                    }
                    break;
                //uf1000i
                case 28:
                    //尿大张
                    if (lr.ParItemList.Contains(90008562))
                    {
                        FillGraphReportItem(lr.SpecItemsTable, ds, 1);
                    }
                    break;
                //遗传
                case 11:
                    //染色体
                    if (lr.ParItemList.Contains(90009044) || lr.ParItemList.Contains(90009045) || lr.ParItemList.Contains(90009046))
                    {
                        FillRanReportItem(lr.SpecItemsList, ds, lr.ReportInfo);
                        FillGraphReportItem(lr.SpecItemsTable, ds, 3);
                    }
                    //fish
                    else
                    {
                        FillGraphReportItem(lr.SpecItemsTable, ds, 2);
                    }
                    break;
                    //Spife4000 血清蛋白电泳
                case 35:
                    FillGraphReportItem(lr.SpecItemsTable, ds, 4);
                    break;
                    //细胞化学
                case 3:
                    FillGraphReportItem(lr.SpecItemsTable, ds, 5);
                    break;
                    //细胞形态
                case 39:
                    FillGSReportItems(ds, lr.SpecItemsTable);
                    FillGSCommonItems(ds, lr.SpecItemsList, lr.ReportInfo);
                    break;
            }
            //通用检验项填充
            FillReportItems(lr, ds);
        }
        //报告信息填充
        private void FillReportInfo(ReportFormModel rfm, DataSet ds)
        {
            //Type reportFormType = rfm.GetType();
            //PropertyInfo property;
            DataTable dt = ds.Tables["ReportForm"];
            DataRow dr = dt.NewRow();
            dr["ReceiveDate"] = rfm.ReceiveDate;
            dr["SectionNo"] = rfm.SectionNo;
            dr["TestTypeNo"] = rfm.TestTypeNo;
            dr["SampleNo"] = rfm.SampleNo;
            dr["SerialNo"] = rfm.SerialNo;
            dr["DeptName"] = rfm.DeptName;
            dr["BedNo"] = rfm.BedNo;
            dr["CName"] = rfm.CName;
            dr["Sex"] = rfm.Sex;
            dr["Age"] = rfm.Ager.ToString();
            dr["PID"] = rfm.PID;
            dr["SampleTypeName"] = rfm.SampleTypeName;
            dr["ClinicTypeName"] = rfm.ClinicTypeName;
            dr["DoctorName"] = rfm.BillingDoctor;
            dr["ClinicalDiagnosis"] = rfm.ClinicalDiagnosis;
            dr["Explanation"] = rfm.Explanation;
            dr["ParItemName"] = rfm.ParItemName;
            dr["TestDateTime"] = rfm.TestDateTime.ToString("yyyy-MM-dd");

            dr["CollectDateTime"] = FormatDateTime(rfm.CollectDateTime, "yyyy-MM-dd HH:mm", "");
            dr["InceptDateTime"] = FormatDateTime(rfm.InceptDateTime, "yyyy-MM-dd HH:mm", "");
            dr["ReportDateTime"] = FormatDateTime(rfm.ReportDateTime, "yyyy-MM-dd HH:mm", "");

            dr["ReportTitle"] = "";
            dr["Remark"] = rfm.Remark;
            dr["FormMemo"] = rfm.FormMemo;
            dr["FormComment"] = rfm.FormComment;
            dr["FormComment2"] = rfm.FormComment2;

            dr["TechnicianImage"] = rfm.TechnicianImage;
            dr["CheckerImage"] = rfm.CheckerImage;

            dt.Rows.Add(dr);
        }
        //报告项集合填充
        private void FillReportItems(List<ReportItemModel> rimList,ReportFormModel rfm, DataSet ds)
        {
            DataTable dt = ds.Tables["ReportItem"];
            DataRow dr;
            foreach (ReportItemModel rim in rimList)
            {
                //过滤不显示项
                if (rim.SecretGrade > 0)
                {
                    continue;
                }
                dr = dt.NewRow();
                FillReportItem(rim, dr, rfm);
                dt.Rows.Add(dr);
            }
        }
        private void FillReportItems(LisReport lr, DataSet ds)
        {
            DataTable dt = ds.Tables["ReportItem"];
            DataRow dr;
            foreach (ReportItemModel rim in lr.ReportItemList)
            {
                //过滤不显示项
                if (rim.SecretGrade > 0 || rim.ItemResult.Equals(""))
                {
                    continue;
                }
                dr = dt.NewRow();
                FillReportItem(rim, dr, lr.ReportInfo);
                dt.Rows.Add(dr);
            }
        }
       //通用报告项数据填充
        private void FillReportItem(ReportItemModel rim, DataRow dr, ReportFormModel rfm)
        {
            dr["ReceiveDate"] = rfm.ReceiveDate;
            dr["SectionNo"] = rfm.SectionNo;
            dr["TestTypeNo"] = rfm.TestTypeNo;
            dr["SampleNo"] = rfm.SampleNo;
            dr["ItemNo"] = rim.ItemNo;
            dr["DispNo"] = rim.DispNo;
            dr["ItemName"] = rim.ItemName;
            if (rim.Precision > 0)
            {
                double temp, temp1;
                bool r = double.TryParse(rim.ItemResult, out temp);
                bool r1 = double.TryParse(rim.ItemEName, out  temp1);
                if (r == true)
                {
                    dr["ItemResult"] = temp.ToString(NumberFormatString(rim.Precision));
                }
                else
                {
                    dr["ItemResult"] = rim.ItemResult;
                }
                if (r1 == true)
                {
                    dr["ItemEName"] = temp1.ToString(NumberFormatString(rim.Precision));
                }
                else
                {
                    dr["ItemEName"] = rim.ItemEName;
                }
            }
            else
            {
                dr["ItemEName"] = rim.ItemEName;
                dr["ItemResult"] = rim.ItemResult;
            }
            dr["ResultStatus"] = rim.ResultStatus;
            dr["ItemUnit"] = rim.ItemUnit;
            dr["RefRange"] = rim.RefRange;
        }
        //数据精度格式获取
        private string NumberFormatString(int prec)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0.");
            for (int i = 0; i < prec; i++)
            {
                sb.Append('0');
            }
            return sb.ToString();
        }
        //血常规人工分类项填充
        private void FillManReportItem(Hashtable manItemTable,DataSet ds)
        {
            DataTable dt = ds.Tables["ManReportItem"];
            DataRow dr = dt.NewRow();
            dr["ReceiveDate"] = manItemTable["receivedate"];
            dr["SectionNo"] = manItemTable["sectionno"];
            dr["TestTypeNo"] = manItemTable["testtypeno"];
            dr["SampleNo"] = manItemTable["sampleno"];
            dr["Item9288"] = manItemTable["Item9288"];
            dr["Item9289"] = manItemTable["Item9289"];
            dr["Item9290"] = manItemTable["Item9290"];
            dr["Item9291"] = manItemTable["Item9291"];
            dr["Item9292"] = manItemTable["Item9292"];
            dr["Item9293"] = manItemTable["Item9293"];
            dr["Item9294"] = manItemTable["Item9294"];
            dr["Item9300"] = manItemTable["Item9300"];
            dr["Item9295"] = manItemTable["Item9295"];
            dr["Item9296"] = manItemTable["Item9296"];
            dr["Item9297"] = manItemTable["Item9297"];
            dr["Item9301"] = manItemTable["Item9301"];
            dt.Rows.Add(dr);
        }
        //遗传染色体检验项填充
        private void FillRanReportItem(List<ReportItemModel> rimList, DataSet ds, ReportFormModel rfm)
        {
            DataTable dt = ds.Tables["RanReportItem"];
            DataRow dr = dt.NewRow();
            dr["ReceiveDate"] = rfm.ReceiveDate;
            dr["SectionNo"] = rfm.SectionNo;
            dr["TestTypeNo"] = rfm.TestTypeNo;
            dr["SampleNo"] = rfm.SampleNo;
            foreach (ReportItemModel rim in rimList)
            {
                switch (rim.ItemNo)
                {
                    case 90008528:
                        dr["CaryogramDesc"] = rim.ItemResult;
                        break;
                    case 90008797:
                        dr["SampleQuality"] = rim.ItemResult;
                        break;
                    case 90008798:
                        dr["CulturalMethod"] = rim.ItemResult;
                        break;
                    case 90008799:
                        dr["BandMethod"] = rim.ItemResult;
                        break;
                    default:
                        dr["Remark"] = rim.ItemResult;
                        break;
                }
            }
            dt.Rows.Add(dr);
        }
        //图像检验项填充
        private void FillGraphReportItem(Hashtable graphItemTable, DataSet ds,int graphOwner)
        {
            DataTable dt = ds.Tables["RFGraphData"];
            DataRow dr = dt.NewRow();
            //通用项填充
            dr["ReceiveDate"] = graphItemTable["receivedate"];
            dr["SectionNo"] = graphItemTable["sectionno"];
            dr["TestTypeNo"] = graphItemTable["testtypeno"];
            dr["SampleNo"] = graphItemTable["sampleno"];
            //图像项填充
            switch (graphOwner)
            {
                //尿大张图
                case 1:
                    dr["S_FLHxS"] = graphItemTable["S_FLHxS"];
                    dr["S_FLLxS"] = graphItemTable["S_FLLxS"];
                    dr["S_SSCxS"] = graphItemTable["S_SSCxS"];
                    dr["B_FLHxB1"] = graphItemTable["B_FLHxB1"];
                    dr["S_FSCWxS"] = graphItemTable["S_FSCWxS"];
                    dr["S_FLLWxS"] = graphItemTable["S_FLLWxS"];
                    break;
                //FISH图
                case 2:
                    dr["FISH_Normal"] = graphItemTable["FISH_Normal"];
                    dr["YiChuan_Result"] = graphItemTable["图像1"];
                    break;
                //染色体图
                case 3:
                    dr["YiChuan_Result"] = graphItemTable["图像1"];
                    break;
                    //Spife4000 血清蛋白电泳图
                case 4:
                    dr["DianYong"] = graphItemTable["蛋白电泳"];
                    dr["TuPu"] = graphItemTable["图谱"];
                    break;
                    //细胞化学 图像
                case 5:
                    dr["FISH_Normal"] = graphItemTable["图像1"];
                    dr["YiChuan_Result"] = graphItemTable["图像2"];
                    break;
            }
            dt.Rows.Add(dr);
        }
        private void FillGSReportItems(DataSet ds,Hashtable table)
        {
            DataTable dt = ds.Tables["GSReportResult"];
            DataRow dr = dt.NewRow();
            //通用项填充
            dr["ReceiveDate"] = table["receivedate"];
            dr["SectionNo"] = table["sectionno"];
            dr["TestTypeNo"] = table["testtypeno"];
            dr["SampleNo"] = table["sampleno"];
            dr["MorphologicalDesc"] = table["MorphologicalDesc"];
            dr["DiagnosticOpinion"] = table["DiagnosticOpinion"];
            dr["Image1"] = table["image1"];
            dr["Image2"] = table["image2"];
            dt.Rows.Add(dr);
        }
        private void FillGSCommonItems(DataSet ds, List<ReportItemModel> rimList, ReportFormModel rfm)
        {
            DataTable dt = ds.Tables["GSReportItem"];
            DataRow dr = dt.NewRow();
            dr["ReceiveDate"] = rfm.ReceiveDate;
            dr["SectionNo"] = rfm.SectionNo;
            dr["TestTypeNo"] = rfm.TestTypeNo;
            dr["SampleNo"] = rfm.SampleNo;
            GSCommonItemModel gsItem;
            string columnName;
            foreach (ReportItemModel rim in rimList)
            {
                gsItem = rim as GSCommonItemModel;
                if (gsItem == null)
                {
                    continue;
                }
                columnName = "i" + gsItem.ItemNo.ToString() + "_" + "xue";
                dr[columnName] = gsItem.XueValue;
                columnName = "i" + gsItem.ItemNo.ToString() + "_" + "gs";
                dr[columnName] = gsItem.GSValue;
            }
            dt.Rows.Add(dr);
        }
        //时间字段格式化
        private string FormatDateTime(DateTime dt, string formatter, string emptyLabel)
        {
            if (dt == DateTime.MinValue)
            {
                return emptyLabel;
            }
            else
            {
                return dt.ToString(formatter);
            }
        }
        //获取报告模板名
        private string GetReportModelName(LisReport lr)
        {
            string modelName;
            switch (lr.PrintModelNo)
            {
                //血常规
                case 100:
                    modelName = "lj-xuechanggui.frx";
                    break;
                //尿大张
                case 200:
                    modelName = "lj-niaodazhang.frx";
                    break;
                //尿小张
                case 300:
                    modelName = "lj-niaoxiaozhang.frx";
                    break;
                //生化大张
                case 400:
                    modelName = "shenghua-Common-da.frx";
                    break;
                //生化小张
                case 500:
                    modelName = "shenghua-01-1.frx";
                    break;
                //tecan150免疫小张
                case 600:
                    modelName = "mianyi-TECAN150-xiao.frx";
                    break;
                //dxi800 免疫大张
                case 610:
                    modelName = "mianyi-DXI800-da-1.frx";
                    break;
                //Spife 4000 大张
                case 620:
                    modelName = "mianyi-SPIFE4000-da.frx";
                    break;
                //免疫普通小张
                case 700:
                    modelName = "mianyi-Common-xiao.frx";
                    break;
                //免疫手工小张
                case 710:
                    modelName = "mianyi-shougong-xiao-1.frx";
                    break;
                //免疫SUNRISE 小张
                case 720:
                    modelName = "mianyi-Sunrise-xiao.frx";
                    break;
                    //免疫细胞培养
                case 730:
                    modelName = "mianyi-xibaopeiyang-xiao.frx";
                    break;
                //出凝血大张
                case 800:
                    modelName = "zhixue-Adaption-1.frx";
                    break;
                //出凝血小张
                case 900:
                    modelName = "zhixue-01-1.frx";
                    break;
                //溶血大张
                case 1000:
                    modelName = "rongxue-Common-da.frx";
                    break;
                //溶血小张
                case 1100:
                    modelName = "rongxue-Common-xiao.frx";
                    break;
                //遗传FISH
                case 1200:
                    modelName = "yichuan-FISH.frx";
                    break;
                //遗传染色体
                case 1300:
                    modelName = "yichuan-Ran.frx";
                    break;
                //细胞化学 单图
                case 1400:
                    modelName = "zuhua-1image-1.frx";
                    break;
                //细胞化学 单图2
                case 1420:
                    modelName = "zuhua-1image-2.frx";
                    break;
                    //细胞化学 多图
                case 1430:
                    modelName = "zuhua-2image-1.frx";
                    break;
                //细胞形态
                case 1450:
                    modelName = "xingtai-01-1.frx";
                    break;
                //ReportType.Rt分子生物I脱氧核糖核酸巨细胞病毒脱氧核糖核酸EB病毒=1500
                case 1500:
                    modelName = "fs-06-175.frx";
                    break;
                case 1510:
                case 1530:
                case 1540:
                case 1520:
                    modelName = "fs-06-02.frx";
                    break;
                //40种，特殊
                case 1550:
                    modelName = "fs-40.frx";
                    break;
                case 1560:
                    modelName = "40";
                    break;
                case 1570:
                    modelName = "40";
                    break;
                case 1600:
                    modelName = "ls-1600.frx";
                    break;
                case 1610:
                    modelName = "ls-1610.frx";
                    break;
                case 1620:
                    modelName = "ls-1620.frx";
                    break;
                case 1630:
                    modelName = "ls-1630.frx";
                    break;
                case 1640:
                    modelName = "ls-1640.frx";
                    break;
                case 1650:
                    modelName = "ls-1650.frx";
                    break;
                case 1660:
                    modelName = "ls-1660.frx";
                    break;
                default:
                    modelName = "lj-03-1.frx";
                    break;
            }
            return modelName;
        }
       //获取模板相对全路径
        private string GetReprotModelPath(string modelName)
        {
            return Path.Combine(System.Environment.CurrentDirectory,"model",modelName);
        }
        //获取打印数据集框架
        private string GetReportDataStructPath(string frdName)
        {
            return Path.Combine(System.Environment.CurrentDirectory, "dataset", frdName);
        }
        //获取生成文件全路径
        private string GenderFileFullName(ReportFormModel rf, string rootPath,int order)
        {
            string fileFullPath=Path.Combine(rootPath, DateTime.Now.ToString("yyyy-MM-dd"), rf.SectionNo.ToString());
            if (!Directory.Exists(fileFullPath))
            {
                Directory.CreateDirectory(fileFullPath);
            }
            string fileName = GenderFileName(rf,order);
            string fileFullName = Path.Combine(fileFullPath, fileName);
            if (File.Exists(fileFullName))
            {
                string newFileName = fileName.Substring(0, fileName.Length - 4);
                newFileName += "_";
                newFileName += DateTime.Now.ToString("HHmmss");
                newFileName += ".pdf";
                return Path.Combine(fileFullPath, newFileName);
            }
            else
            {
                return fileFullName;
            }
        }
        //获取生成文件名
        private string GenderFileName(ReportFormModel rf, int order)
        {
            StringBuilder sb = new StringBuilder();
            //病案号
            if (rf.PID != null && !rf.PID.Equals(""))
            {
                sb.Append(rf.PID);
            }
            else
            {
                sb.Append('%');
            }
            sb.Append('_');
            //身份证号
            if (rf.CID != null && !rf.CID.Equals(""))
            {
                sb.Append(rf.CID);
            }
            else
            {
                sb.Append('%');
            }
            sb.Append('_');
            //lis
            sb.Append("lis");
            sb.Append('_');
            //门诊类型
            if (rf.SickTypeNo == 1 || rf.SickTypeNo == 3)
            {
                //住院
                sb.Append('I');
            }
            else if (rf.SickTypeNo == 2 || rf.SickTypeNo == 4)
            {
                //menzhen 
                sb.Append('O');
            }
            else
            {
                sb.Append('Q');
            }
            sb.Append('_');
            //住院次数
            if (rf.VisitTimes > 0)
            {
                sb.Append(rf.VisitTimes);
            }
            else
            {
                sb.Append('%');
            }
            sb.Append('_');
            //采集时间
            sb.Append(FormatDateTime(rf.CollectDateTime, "yyyyMMdd", "%"));
            sb.Append('_');
            //签收时间
            sb.Append(FormatDateTime(rf.InceptDateTime, "yyyyMMdd", "%"));
            sb.Append('_');
            //报告时间
            sb.Append(FormatDateTime(rf.ReportDateTime, "yyyyMMdd", "%"));
            sb.Append('_');
            //小组号
            sb.Append(rf.SectionNo);
            sb.Append('_');
            //样本号 将\进行转义
            sb.Append(rf.SampleNo.Replace('/','-'));
            sb.Append('_');
            if (rf.SerialNo != null && !rf.SerialNo.Equals(""))
            {
                sb.Append(rf.SerialNo);
            }
            else
            {
                sb.Append('%');
            }
            sb.Append('_');
            sb.Append(order);
            sb.Append(".pdf");
            return sb.ToString();
        }
        //清空dataset
        private void ClearDataSet(DataSet ds)
        {
            ds.Clear();
        }
        private void GenderManReportItem(List<ReportItemModel> rimList, Hashtable manItemTable)
        {
            foreach (ReportItemModel rim in rimList)
            {
                switch (rim.ItemNo)
                {
                    case 90009288:
                        manItemTable.Add("Item9288", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009289:
                        manItemTable.Add("Item9289", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009290:
                        manItemTable.Add("Item9290", rim.ItemResult);
                        break;
                    case 90009291:
                        manItemTable.Add("Item9291", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009292:
                        manItemTable.Add("Item9292", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009293:
                        manItemTable.Add("Item9293", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009294:
                        manItemTable.Add("Item9294", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009300:
                        manItemTable.Add("Item9300", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009295:
                        manItemTable.Add("Item9295", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009296:
                        manItemTable.Add("Item9296", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009297:
                        manItemTable.Add("Item9297", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                    case 90009301:
                        manItemTable.Add("Item9301", rim.ItemResult);
                        rimList.Remove(rim);
                        break;
                }
            }
        }
        private Hashtable GenderItemEqualFields(ReportFormModel rfm)
        {
            Hashtable ht = new Hashtable();
            ht.Add("receivedate", rfm.ReceiveDate);
            ht.Add("sectionno", rfm.SectionNo);
            ht.Add("testtypeno", rfm.TestTypeNo);
            ht.Add("sampleno", rfm.SampleNo);
            return ht;
        }
        private Hashtable GenderGraphReportItem(Hashtable graphItemTable)
        {
            RFGraphDataDAL rfdal = new RFGraphDataDAL();
            return rfdal.SearchTable(graphItemTable);
        }
        private void FillSpecialReportItem(Hashtable graphItemTable, DataSet ds, string tableName)
        {
            DataTable dt = ds.Tables["tableName"];
            DataRow dr = dt.NewRow();
            foreach (DictionaryEntry de in graphItemTable)
            {
                dr[de.Key.ToString()] = de.Value;
            }
        }
        private string getliushiname(string SerialNo, string SampleNo, string rootPath)
        {
            DirectoryInfo TheFoldercs = new DirectoryInfo(rootPath + "\\liushi\\");
            foreach (var item in TheFoldercs.GetFiles())
            {
                if (item.Name.Contains(SerialNo + "_" + SampleNo))
                    return item.Name;
            }
            //这里要询问一下是否每个都带图
            return "";
        }

        private bool AfterCreatPDF(int zuhao)
        {
            switch (zuhao)
            {
                case 10:
                    return true;
                default:
                    return false;
            }
        }

        private void mergePDFFiles(List<MyMergePdf> fileList, string outMergeFile)
        {
            PdfReader reader;
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outMergeFile, FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            PdfImportedPage newPage;
            foreach (MyMergePdf item in fileList)
            {
                reader = new PdfReader(item.Name);
                int iPageNum;
                if (item.PagCount == 0)
                {
                    iPageNum = reader.NumberOfPages;
                }
                else
                {
                    iPageNum = item.PagCount;
                }
                for (int j = 1; j <= iPageNum; j++)
                {
                    document.NewPage();
                    newPage = writer.GetImportedPage(reader, j);
                    cb.AddTemplate(newPage, 0, 0);
                }
            }
            document.Close();
        }
        
        #endregion
    }
}
