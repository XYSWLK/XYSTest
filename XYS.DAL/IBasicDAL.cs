using System;
using System.Collections;
using System.Collections.Generic;
namespace XYS.DAL
{
    public interface IBasicDAL<T>
    {
        T Search(Hashtable equalFields);

        List<T> SearchList(Hashtable equalFields);

    }
}
