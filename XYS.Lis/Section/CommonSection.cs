using System;
using System.Collections;

namespace XYS.Lis.Section
{
    public class CommonSection
    {
        private static readonly Hashtable SectionOrder=new Hashtable();
        public CommonSection()
        {
        }
        public static int GetDisplayOrder(int sectionno)
        {
            if (SectionOrder.Count == 0)
            {
                InitDisplayOrder();
            }
            if (SectionOrder[sectionno] != null)
            {
                return (int)SectionOrder[sectionno];
            }
            else
            {
                return -1;
            }
        }
        private  static void InitDisplayOrder()
        {
            //临检
            SectionOrder.Add(28, 10);
            SectionOrder.Add(62, 10);
            //止血
            SectionOrder.Add(4, 11);
            //生化
            SectionOrder.Add(17, 12);
            SectionOrder.Add(23, 12);
            SectionOrder.Add(29, 12);
            SectionOrder.Add(34, 12);
            //免疫
            SectionOrder.Add(5, 13);
            SectionOrder.Add(19, 13);
            SectionOrder.Add(20, 13);
            SectionOrder.Add(21, 13);
            SectionOrder.Add(25, 13);
            SectionOrder.Add(30, 13);
            SectionOrder.Add(33, 13);
            SectionOrder.Add(35, 13);
            SectionOrder.Add(63, 13);
            //细胞培养
            SectionOrder.Add(14, 15);
            //溶血
            SectionOrder.Add(18, 15);
        }
    }
}
