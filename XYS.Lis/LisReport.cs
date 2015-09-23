using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XYS.Lis.Model;
namespace XYS.Lis
{
   public class LisReport
    {
       #region 私有字段
       
       private string _reportTitle;
       private int _orderNo; //排序号
       private int _printModelNo;//模板号
       private bool _specItemFlag;
       private ReportFormModel _reportInfo;
       private readonly List<ReportItemModel> _reportItemList;
       private readonly Hashtable _specItemsTable;
       private readonly List<ReportItemModel> _specItemsList;
       private readonly List<int> _parItemList;

       #endregion

       #region 公共构造函数
       public LisReport() {
           this._reportItemList = new List<ReportItemModel>();
           this._specItemsTable=new Hashtable();
           this._parItemList = new List<int>();
           this._reportInfo = new ReportFormModel();
           this._specItemsList = new List<ReportItemModel>();
       }

       #endregion

       #region 公共属性

       public string ReportTitle
       {
           get { return this._reportTitle; }
           set { this._reportTitle = value; }
       }
       public ReportFormModel ReportInfo
       {
           get { return this._reportInfo; }
           set { this._reportInfo = value; }
       }
       public List<ReportItemModel> ReportItemList
       {
           get { return this._reportItemList; }
       }
       public Hashtable SpecItemsTable
       {
           get{return this._specItemsTable;}
       }
       public int OrderNo
       {
           get { return this._orderNo; }
           set { this._orderNo = value; }
       }
       public int PrintModelNo
       {
           get { return this._printModelNo; }
           set { this._printModelNo = value; }
       }
       public void AddReportItem(ReportItemModel rim)
       {
           this._reportItemList.Add(rim);
       }
       public List<int> ParItemList
        {
            get { return this._parItemList; }
        }
       public List<ReportItemModel> SpecItemsList
       {
           get { return this._specItemsList; }
       }
       public void SortReportItems()
       {
           this._reportItemList.Sort();
       }
       public bool SpecItemFlag
       {
           get { return this._specItemFlag; }
           set { this._specItemFlag = value; }
       }
       #endregion

        #region 公共方法
       public void Clear()
       {
           this._reportItemList.Clear();
           this._parItemList.Clear();
           this._specItemsTable.Clear();
           this._specItemsList.Clear();
           this._specItemFlag = false;
           this._orderNo = 0;
           this._printModelNo = -1;
       }
        #endregion
    }
}
