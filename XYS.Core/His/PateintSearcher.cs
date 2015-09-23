using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;

namespace XYS.His
{
    public class PateintSearcher:IPatientSearcher
    {
        #region 私有字段

        private string _name;
        private SearchType _category;
        private long _value;
        #endregion

        #region 公共构造方法

        public PateintSearcher()
        {
            this._name = "病人查询器";
            this._category = SearchType.PatientSearch;
        }

        #endregion

        #region 实现ISearch接口属性

        public string Name
        {
            get {return this._name; }
        }
        public SearchType SearchCategory
        {
            get { return this._category; }
        }
        public long Value
        {
            get { return this._value; }
        }
        #endregion

        #region 实现IPatientSearcher接口方法
        public List<IResultModel> getPatientInfo(SearchArgument argument)
        {
            throw new NotImplementedException();
        }
        public bool isLocalPatient(string pid)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 受保护的内部虚方法
        #endregion

        public event PatientInfoQueryHandler PatientInfoQueryEvent;

    }
}
