using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XYS.Utility.DB;
using System.IO;
using System.Xml;
using XYS.Lis.Utility;
using XYS.Lis;

using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ZhTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReportManager rm = new ReportManager();
            PDFManager pm = new PDFManager();
            LisReport lr = new LisReport();
            Hashtable equalFields = new Hashtable();
            string sql = "select receivedate,sectionno,testtypeno,sampleno from ReportForm where SectionNo=" + textBox1.Text.Trim() + " and checkdate='"+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            DataSet ds = pm.GetPrintDataSet("ReportTables.frd");
            foreach (DataRow dr in dt.Rows)
            {
                equalFields.Clear();
                equalFields.Add("receivedate", (DateTime)dr["receivedate"]);
                equalFields.Add("sectionno", (int)dr["sectionno"]);
                equalFields.Add("testtypeno", (int)dr["testtypeno"]);
                equalFields.Add("sampleno", dr["sampleno"].ToString());
                rm.InitLisReport(lr, equalFields);
                pm.GenderPDFReport(lr, "E:\\lis", ds);
            }
        }
        private static void ConvertDataSetToXMLFile(DataSet xmlDS, string xmlFile)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //用WriteXml方法写入文件.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                //返回Unicode编码的文本
                UnicodeEncoding utf = new UnicodeEncoding();
                StreamWriter sw = new StreamWriter(xmlFile);
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sw.WriteLine(utf.GetString(arr).Trim());
                sw.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        private static DataSet ConvertXMLFileToDataSet(string xmlFile)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.Load(xmlFile);

                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmld.InnerXml);
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                //xmlDS.ReadXml(reader);
                xmlDS.ReadXmlSchema(reader);
                //xmlDS.ReadXml(xmlFile);
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = XMLTools.ConvertFRDFile2DataSet("ReportTables.frd");
            this.textBox1.Text = ds.Tables.Count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReportManager rm = new ReportManager();
            PDFManager pm = new PDFManager();
            LisReport lr = new LisReport();
            Hashtable equalFields = new Hashtable();
            string sql = "select receivedate,sectionno,testtypeno,sampleno from ReportForm where receivedate='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and Sectionno in(3,4,11,14,17,23,29,34,5,19,20,21,25,30,33,35,63,18,2,27,28,62)";
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            DataSet ds = pm.GetPrintDataSet("ReportTables.frd");
            foreach (DataRow dr in dt.Rows)
            {
                equalFields.Clear();
                equalFields.Add("receivedate", (DateTime)dr["receivedate"]);
                equalFields.Add("sectionno", (int)dr["sectionno"]);
                equalFields.Add("testtypeno", (int)dr["testtypeno"]);
                equalFields.Add("sampleno", dr["sampleno"].ToString());
                rm.InitLisReport(lr, equalFields);
                pm.GenderPDFReport(lr, LisConfig.GetPdfRootPath(), ds);
            }
            //GenderRemoteFile(dateTimePicker1.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReportManager rm = new ReportManager();
            PDFManager pm = new PDFManager();
            LisReport lr = new LisReport();
            Hashtable equalFields = new Hashtable();
            string sql = "select receivedate,sectionno,testtypeno,sampleno from ReportForm where SerialNo='" + textBox2.Text.Trim() + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            DataSet ds = pm.GetPrintDataSet("ReportTables.frd");
            foreach (DataRow dr in dt.Rows)
            {
                equalFields.Clear();
                equalFields.Add("receivedate", (DateTime)dr["receivedate"]);
                equalFields.Add("sectionno", (int)dr["sectionno"]);
                equalFields.Add("testtypeno", (int)dr["testtypeno"]);
                equalFields.Add("sampleno", dr["sampleno"].ToString());
                rm.InitLisReport(lr, equalFields);
                pm.GenderPDFReport(lr, "E:\\lis", ds);
                // lr = rm.GetLisReport(equalFields);
                //pm.GenderPDFReport(lr, "E:\\lis");
            }
        }
        private void GenderRemoteFile(DateTime dt)
        {
            string directoryPath = Path.Combine(LisConfig.GetPdfRootPath(), DateTime.Now.ToString("yyyy-MM-dd"));
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            DirectoryInfo[] sectionDirectories = di.GetDirectories();
            string[] filePaths;
            int sectionNo;
            foreach(DirectoryInfo d in sectionDirectories)
            {
                if (int.TryParse(d.Name, out sectionNo))
                {
                    filePaths = Directory.GetFiles(d.FullName);
                    CombinePDF(filePaths,dt,sectionNo);
                }
            }
        }
        private void CombinePDF(string[] filePaths,DateTime dt,int sectionNo)
        {
            PdfDocument outPdf = new PdfDocument();
            for (int i = 0; i < filePaths.Length; i++)
            {
                PdfDocument one = PdfReader.Open(filePaths[i], PdfDocumentOpenMode.Import);
                CopyPages(one, outPdf);
            }
            string remotePath = Path.Combine(LisConfig.GetCombineRootPath(), LisConfig.GetSectionsName(sectionNo), dt.ToString("yyyy-MM-dd") + ".pdf");
            outPdf.Save(remotePath);
        }
        private void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }
    }
}
