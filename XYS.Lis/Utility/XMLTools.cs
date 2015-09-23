using System;
using System.Xml;
using System.Data;

namespace XYS.Lis.Utility
{
    public class XMLTools
    {
        #region 公共静态方法
        
        //根据xml文件生成dataset
        public static DataSet ConvertFRDFile2DataSet(string frdFile)
        {
            DataSet ds = new DataSet();
            XmlDocument doc = new XmlDocument();
            doc.Load(frdFile);
            XmlNode root = doc.SelectSingleNode("Dictionary");
            //设置table
            XmlNodeList XmlTables = root.SelectNodes("TableDataSource");
            DataTable dt;
            for (int i = 0; i < XmlTables.Count; i++)
            {
                XmlNode node= XmlTables[i];
                dt = ConvertXml2DataTable(node);
                ds.Tables.Add(dt);
            }
            //设置 ralation
            XmlNodeList XmlRelations = root.SelectNodes("Relation");
            foreach (XmlNode node in XmlRelations)
            {
                ConvertXml2TableRelation(node, ds);
            }
            return ds;
        }

        #endregion
        #region 静态私有方法
       
        //设置数据表
        private static DataTable ConvertXml2DataTable(XmlNode node)
        {
            string TableName = node.Attributes["Name"].Value;
            DataTable dt = new DataTable();
            dt.TableName = TableName;
            XmlNodeList XmlColumns = node.ChildNodes;
            string ColumnName;
            string ColumnType;
            foreach (XmlNode n in XmlColumns)
            {
                ColumnName = n.Attributes["Name"].Value;
                ColumnType = n.Attributes["DataType"].Value;
                dt.Columns.Add(ColumnName, Type.GetType(ColumnType));
            }
            return dt;
        }
        //设置数据表关系
        private static void ConvertXml2TableRelation(XmlNode node, DataSet ds)
        {
            string relationName = node.Attributes["Name"].Value;
            string parentTableName = node.Attributes["ParentDataSource"].Value;
            string childTableName = node.Attributes["ChildDataSource"].Value;
            string[] parentColumnsName = node.Attributes["ParentColumns"].Value.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] childColumnsName = node.Attributes["ChildColumns"].Value.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            ds.Relations.Add(relationName, GetRelationColumns(ds.Tables[parentTableName],parentColumnsName),GetRelationColumns(ds.Tables[childTableName],childColumnsName));
        }
        //获取关系列
        private static DataColumn[] GetRelationColumns(DataTable dt,string[] relationColumnsName)
        {
            DataColumn[] relationColumns=new DataColumn[relationColumnsName.Length];
            int relationColumnIndex = 0;
            foreach (string columnName in relationColumnsName)
            {
                relationColumns[relationColumnIndex] = dt.Columns[columnName];
                relationColumnIndex++;
            }
            return relationColumns;
        }

        #endregion
    }
}
