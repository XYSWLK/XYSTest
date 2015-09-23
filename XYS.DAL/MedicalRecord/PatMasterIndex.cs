using System;
using System.Collections.Generic;
using System.Data;
using XYS;
using XYS.DAL;
using XYS.Model;
namespace XYS.DAL.MedicalRecord
{
    public class PatMasterIndex:IRecord
    {
        #region 私有字段

        private string _name;

        #endregion

        #region 实现IRecord接口属性
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region 实现IRecord接口方法
        public List<IResultModel> Search(SearchArgument argument)
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete<T>(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update<T>(T t)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 受保护的内部虚方法
        #endregion

        #region 私有方法
        private List<IResultModel> DateTableToList(DataTable dt)
        {
            List<IResultModel> result = new List<IResultModel>();
            foreach (DataRow dr in dt.Rows)
            {
 
            }
            return result;
        }
        #endregion
    }
}
