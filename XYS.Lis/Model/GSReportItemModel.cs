using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Model
{
    public class GSReportItemModel:ReportItemModel
    {
        private string _reportText;
        private int _isFile;
        private string _filePath;
        public string ReportText
        {
            get { return this._reportText; }
            set { this._reportText = value; }
        }

        public int IsFile
        {
            get { return this._isFile; }
            set { this._isFile = value; }
        }
        public string FilePath
        {
            get { return this._filePath; }
            set { this._filePath = value; }
        }
    }
}
