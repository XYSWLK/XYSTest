using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Model
{
    public class ReportItemModel : IComparable<ReportItemModel>
    {
        #region 私有字段

        private int _dispNo;
        private int _itemNo;
        private string _itemName;
        private string _itemResult;
        private string _resultStatus;
        private string _itemUnit;
        private string _refRange;
        private int _secretGrade;
        private string _itemEName;
        //private DateTime _receiDate;
        // private int _sectionNo;
        //private int _testTypeNo;
        //private string _sampleNo;
        private int _parItemNo;
        private int _precision;
        #endregion

        #region 公共属性

        public int DispNo
        {
            get { return _dispNo; }
            set { _dispNo = value; }
        }

        public int ItemNo
        {
            get { return _itemNo; }
            set { _itemNo = value; }
        }

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public string ItemEName
        {
            get { return this._itemEName; }
            set { this._itemEName = value; }
        }

        public string ItemResult
        {
            get { return _itemResult; }
            set { _itemResult = value; }
        }

        public string ResultStatus
        {
            get { return _resultStatus; }
            set { _resultStatus = value; }
        }

        public string ItemUnit
        {
            get { return _itemUnit; }
            set { _itemUnit = value; }
        }

        public string RefRange
        {
            get { return _refRange; }
            set { _refRange = value; }
        }
        public int Precision
        {
            get { return this._precision; }
            set { this._precision = value; }
        }
        //public DateTime ReceiveDate
        //{
        //    get { return this._receiDate; }
        //    set { this._receiDate = value; }
        //}
        //public int SectionNo
        //{
        //    get { return this._sectionNo; }
        //    set { this._sectionNo = value; }
        //}

        //public int TestTypeNo
        //{
        //    get { return this._testTypeNo; }
        //    set { this._testTypeNo = value; }
        //}
        //public string SampleNo
        //{
        //    get { return this._sampleNo; }
        //    set { this._sampleNo = value; }
        //}

        public int SecretGrade
        {
            get { return this._secretGrade; }
            set { this._secretGrade = value; }
        }

        public int ParItemNo
        {
            get { return this._parItemNo; }
            set { this._parItemNo = value; }
        }
        #endregion

        public int CompareTo(ReportItemModel model)
        {
            if (model == null)
            {
                return 1;
            }
            else
            {
                return this._dispNo - model.DispNo;
            }
        }
    }
}
