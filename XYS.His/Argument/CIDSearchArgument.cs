using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYS;
namespace XYS.His.Argument
{
    public class CIDSearchArgument:SearchArgument
    {
        private string _cid;
        public CIDSearchArgument()
            : base("身份证号参数", 200L)
        { }
        public CIDSearchArgument(string cid)
            : base("身份证号参数", 200L)
        {
            this._cid = cid;
        }
        public string CID
        {
            set { this._cid = value; }
            get { return this._cid; }
        }
    }
}
