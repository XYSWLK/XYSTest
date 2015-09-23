using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS
{
    public class SearchArgument
    {
        private long _value;
        private string _name;
        protected internal SearchArgument()
        {
            this._value = long.MaxValue;
            this._name = "NULL";
        }
        protected internal SearchArgument(string name, long value)
        {
            this._name = name;
            this._value = value;
        }
        public string Name
        {
            get { return this._name; }
        }
        public long Value
        {
            get { return this._value; }
        }
    }
}
