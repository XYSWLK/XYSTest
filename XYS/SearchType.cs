using System;
namespace XYS
{
    public sealed class SearchType
    {
        private readonly int _searchTypeValue;
        private readonly string _searchTypeName;
        private readonly string _searchTypeDisplayName;
        public SearchType(int searchType, string searchTypeName, string searchTypeDisplayName)
        {
            if (searchTypeName == null)
            {
                throw new ArgumentNullException("searchTypeName");
            }
            if (searchTypeDisplayName == null)
            {
                throw new ArgumentNullException("searchTypeDisplayName");
            }
            this._searchTypeValue = searchType;
            this._searchTypeName = searchTypeName;
            this._searchTypeDisplayName = searchTypeDisplayName;
        }
        public SearchType(int searchType, string searchTypeName)
            : this(searchType, searchTypeName, searchTypeName)
        { }
        public int Value
        {
            get { return this._searchTypeValue; }
        }
        public string Name
        {
            get { return this._searchTypeName; }
        }
        public string DisplayName
        {
            get { return this._searchTypeDisplayName; }
        }
        public readonly static SearchType Non = new SearchType(int.MinValue, "NONE", "不存在");
        public readonly static SearchType PatientSearch = new SearchType(10000, "PatientSearch", "病人查询");
        public readonly static SearchType All = new SearchType(int.MaxValue, "ALL", "所有查询");

    }
}
