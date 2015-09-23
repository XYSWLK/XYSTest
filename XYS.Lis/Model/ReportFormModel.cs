using System;
using System.Collections.Generic;
using XYS.Model;

namespace XYS.Lis.Model
{
    public class ReportFormModel : PatientModel
    {
        #region 私有字段
        private string _serialNo;
        private string _sampleNo;
        private int _sampleTypeNo;
        private string _sampleTypeName;
        private string _billingDoctor;
        private string _clinicalDiagnosis;
        private string _explanation;
        //采集、签收、审核时间,三个重要的时间
        private DateTime _collectDateTime;
        private DateTime _inceptDateTime;
        private DateTime _reportDateTime;
        //报告附注信息
        private DateTime _testDateTime;
        private string _parItemName;
        //门诊类型编号
        private int _sickTypeNo;
        //备注、结论、解释等
        private string _zdy5;
        private string _remark;
        private string _formMemo;
        private string _formComment;
        private string _formComment2;
        //检验者，审核者
        private string _technician;
        private string _checker;
        private byte[] _technicianImage;
        private byte[] _checkerImage;
        //主键
        private DateTime _receiveDate;
        private int _sectionNo;
        private int _testTypeNo;
        #endregion

        #region 公共属性

        public string SerialNo
        {
            get { return _serialNo; }
            set { _serialNo = value; }
        }
        public string SampleNo
        {
            get { return _sampleNo; }
            set { _sampleNo = value; }
        } 

        public string SampleTypeName
        {
            get { return _sampleTypeName; }
            set { _sampleTypeName = value; }
        }
      
        public string BillingDoctor
        {
            get { return _billingDoctor; }
            set { _billingDoctor = value; }
        }

        public string ClinicalDiagnosis
        {
            get { return _clinicalDiagnosis; }
            set { _clinicalDiagnosis = value; }
        }

        public string Explanation
        {
            get { return this._explanation; }
            set { this._explanation = value; }
        }

        public DateTime CollectDateTime
        {
            get { return this._collectDateTime; }
            set { this._collectDateTime = value; }
        }

        public DateTime InceptDateTime
        {
            get { return this._inceptDateTime; }
            set { this._inceptDateTime = value; }
        }

        public DateTime TestDateTime
        {
            get { return this._testDateTime; }
            set { this._testDateTime = value; }
        }
        public DateTime ReportDateTime
        {
            get { return this._reportDateTime; }
            set { this._reportDateTime = value; }
        }

        public string ParItemName
        {
            get { return this._parItemName; }
            set { this._parItemName = value; }
        }

        public int SectionNo
        {
            get { return this._sectionNo; }
            set { this._sectionNo = value; }
        }

        public int TestTypeNo
        {
            get { return this._testTypeNo; }
            set { this._testTypeNo = value; }
        }

        public int SickTypeNo
        {
            get { return this._sickTypeNo; }
            set { this._sickTypeNo = value; }
        }
        public int SampleTypeNo
        {
            get { return this._sampleTypeNo; }
            set { this._sampleTypeNo = value; }
        }
        public string ZDY5
        {
            get { return this._zdy5; }
            set { this._zdy5 = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        public string FormMemo
        {
            get { return this._formMemo; }
            set { this._formMemo = value; }
        }

        public string FormComment
        {
            get { return this._formComment; }
            set { this._formComment = value; }
        }

        public string FormComment2
        {
            get { return this._formComment2; }
            set { this._formComment2 = value; }
        }
        public string Technician
        {
            get { return this._technician; }
            set { this._technician = value; }
        }

        public string Checker
        {
            get { return this._checker; }
            set { this._checker = value; }
        }
        public DateTime ReceiveDate
        {
            get { return this._receiveDate; }
            set { this._receiveDate = value; }
        }
        public byte[] TechnicianImage
        {
            get { return this._technicianImage; }
            set { this._technicianImage = value; }
        }

        public byte[] CheckerImage
        {
            get { return this._checkerImage; }
            set { this._checkerImage = value; }
        }
        #endregion

        #region 公共构造函数

        public ReportFormModel()
        {
         
        }
        #endregion
    }
}
