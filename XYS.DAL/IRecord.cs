using System.Collections.Generic;
using XYS.Model;
using XYS;
namespace XYS.DAL
{
    public interface IRecord
    {
        string Name { get; }
        List<IResultModel> Search(SearchArgument argument);
        bool Add<T>(T t);
        bool Delete<T>(T t);
        bool Update<T>(T t);
    }
}
