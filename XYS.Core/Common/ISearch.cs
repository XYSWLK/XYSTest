using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace XYS.Common
{
    public interface ISearch
    {
        string Name { get; }
        long Value {get;}
        SearchType SearchCategory { get; }
    }
}
