using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace XYS.Model
{
    public enum AgeUnit
    {
        year = 1, month, day,hour
    }
    public class Age:ICloneable
    {
        private int _value;
        private AgeUnit _unit;
        public Age()
        {
            this._unit = AgeUnit.year;
        }
        public Age(int old)
            : this()
        {
            this._value = old;
        }
        public Age(int old, AgeUnit unit)
        {
            this._value = old;
            this._unit = unit;
        }
        public int Value
        {

            get { return this._value; }
            set { this._value = value; }
        }
        public AgeUnit Unit
        {
            get { return this._unit; }
            set { this._unit = value; }
        }
        public override string ToString()
        {
            switch (this._unit)
            {
                case AgeUnit.year:
                    return this._value.ToString() + " 岁";
                case AgeUnit.month:
                    return this._value.ToString() + " 月";
                case AgeUnit.day:
                    return this._value.ToString() + " 天";
                default:
                    return this._value.ToString() + " 岁";
            }
        }
        public Object Clone()
        {
            return (Object)this.MemberwiseClone();
        }
    }
    public class PersonModel:ICloneable
    {
        private Age _personAge;
        private string _name;
        private string _birth;
        private string _nation;
        private string _sex;
        private string _CID;
        public PersonModel()
        {
            this._personAge = new Age();
        }
        private PersonModel(Age age)
        {
            this._personAge = (Age)age.Clone();
        }
        public string CName
        {
            get { return this._name; }
            set { this._name = value; }
        }
        public string Birth
        {
            get { return this._birth; }
            set { this._birth = value; }
        }
        public string Nation
        {
            get { return this._nation; }
            set { this._nation = value; }
        }
        public string Sex
        {
            get { return this._sex; }
            set { this._sex = value; }
        }
        public string CID
        {
            get { return this._CID; }
            set { this._CID = value; }
        }
        public Age Ager
        {
            get { return this._personAge; }
            set
            {
                this._personAge.Value = value.Value;
                this._personAge.Unit = value.Unit;
            }
        }
        public Object Clone()
        {
            PersonModel obj = new PersonModel(this._personAge);
            obj.CName = this.CName;
            obj.Birth = this.Birth;
            obj.Sex = this.Sex;
            obj.Nation = this.Nation;
            return obj;
        }

    }
}
