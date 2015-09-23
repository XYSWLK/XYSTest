using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYS.Model;
namespace XYS.His.Model
{
    public class ResultPatModel:PersonModel,IResultModel
    {
        #region 私有字段

        private string _modelName;
        private long _value;

        private string _patientID;
        private string _identify;
        private bool _medicalRecord;

        #endregion

        #region 实现IResultModel接口属性

        public string ModelName
        {
            get { return this._modelName; }
        }

        public long ModelValue
        {
            get { return this._value; }
        }

        #endregion

        #region 属性
        public string PID
        {
            set { this._patientID = value; }
            get { return this._patientID; }
        }
        public string Identify
        {
            set { this._identify = value; }
            get { return this._identify; }
        }
        public bool MedicalRecord
        {
            set { this._medicalRecord = value; }
            get { return this._medicalRecord; }
        }

        #endregion

    }
}
