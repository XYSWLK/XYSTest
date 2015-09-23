using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Model
{
    public class GSCommonItemModel:ReportItemModel
    {
        private string _xueValue;
        private string _gsValue;
        public string XueValue
        {
            get { return _xueValue; }
            set { _xueValue = value; }
        }
        public string GSValue
        {
            get { return _gsValue; }
            set { _gsValue = value; }
        }
    }
}
