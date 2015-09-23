using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Configuration;
namespace XYS.Lis.Utility
{
    public class LisConfig
    {
        private static readonly string COMBINEROOTPATH = ConfigurationManager.AppSettings["CombineRootPath"];
        private static readonly string COMBINESECTIONS = ConfigurationManager.AppSettings["CombineSections"];
        private static readonly string PDFFILEROOTPATH = ConfigurationManager.AppSettings["PdfFileRootPath"];
        private static readonly string GSRemotePath = ConfigurationManager.AppSettings["GSRemotePath"];
        private static readonly Hashtable COMBINESECTIONTABLE = new Hashtable();
        public static string GetPdfRootPath()
        {
            return LisConfig.PDFFILEROOTPATH;
        }
        public static string GetCombineRootPath()
        {
            return LisConfig.COMBINEROOTPATH;
        }
        public static string GetGSRemotePath()
        {
            return LisConfig.GSRemotePath;
        }
        public static string GetSectionsName(int sectionNo)
        {
            if (LisConfig.COMBINESECTIONTABLE.Count == 0)
            {
                InitSectionsTable();
            }
            if (LisConfig.COMBINESECTIONTABLE[sectionNo] == null)
            {
                return "default";
            }
            else
            {
                return LisConfig.COMBINESECTIONTABLE[sectionNo].ToString();
            }
        }
        private static void InitSectionsTable()
        {
            string[] sectionArray = LisConfig.COMBINESECTIONS.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
            string[] sectionInfo;
            int sectionNo;
            foreach (string s in sectionArray)
            {
                sectionInfo = s.Split(new char[]{':'},StringSplitOptions.RemoveEmptyEntries);
                if (sectionInfo.Length < 2)
                {
                    continue;
                }
                if (int.TryParse(sectionInfo[0].Trim(), out sectionNo))
                {
                    LisConfig.COMBINESECTIONTABLE.Add(sectionNo, sectionInfo[1].Trim());
                }
            }
        }
    }
}
