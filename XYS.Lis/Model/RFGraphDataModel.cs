using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Model
{
    public class RFGraphDataModel
    {
        private DateTime _receiveDate;
        private int _sectionNo;
        private int _testTypeNo;
        private string _sampleNo;
        private int _graphNo;
        private string _graphName;
        private byte[] _graphImage;
        //private byte[] _b_FLHxB2;
        //private byte[] _b_FSCWxB;
        //private byte[] _rBC_s;
        //private byte[] _s_FLHxS;
        //private byte[] _s_FLLWxS;
        //private byte[] _s_FLLxS;
        //private byte[] _s_FSCWxS;
        //private byte[] _s_SSCxS;
        //private byte[] _sPERM_S;
        //private byte[] _wBC_S;

      

        public DateTime ReceiveDate
        {
            get { return this._receiveDate; }
            set { this._receiveDate = value; }
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

        public string SampleNo
        {
            get { return this._sampleNo; }
            set { this._sampleNo = value; }
        }

        public int GraphNo
        {
            get { return this._graphNo; }
            set { this._graphNo = value; }
        }

        public string GraphName
        {
            get { return this._graphName; }
            set { this._graphName = value; }
        }
        public byte[] GraphImage
        {
            get { return this._graphImage; }
            set { this._graphImage = value; }
        }
        //public byte[] B_FLHxB1
        //{
        //    get { return _b_FLHxB1; }
        //    set { _b_FLHxB1 = value; }
        //}
        //public byte[] B_FLHxB2
        //{
        //    get { return _b_FLHxB2; }
        //    set { _b_FLHxB2 = value; }
        //}
        //public byte[] B_FSCWxB
        //{
        //    get { return _b_FSCWxB; }
        //    set { _b_FSCWxB = value; }
        //}
        //public byte[] RBC_s
        //{
        //    get { return _rBC_s; }
        //    set { _rBC_s = value; }
        //}
        //public byte[] S_FLHxS
        //{
        //    get { return _s_FLHxS; }
        //    set { _s_FLHxS = value; }
        //}
        //public byte[] S_FLLWxS
        //{
        //    get { return _s_FLLWxS; }
        //    set { _s_FLLWxS = value; }
        //}
        //public byte[] S_FLLxS
        //{
        //    get { return _s_FLLxS; }
        //    set { _s_FLLxS = value; }
        //}
        //public byte[] S_FSCWxS
        //{
        //    get { return _s_FSCWxS; }
        //    set { _s_FSCWxS = value; }
        //}
        //public byte[] S_SSCxS
        //{
        //    get { return _s_SSCxS; }
        //    set { _s_SSCxS = value; }
        //}
        //public byte[] SPERM_S
        //{
        //    get { return _sPERM_S; }
        //    set { _sPERM_S = value; }
        //}
        //public byte[] WBC_S
        //{
        //    get { return _wBC_S; }
        //    set { _wBC_S = value; }
        //}
    }
}
