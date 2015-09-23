using System;
using System.Collections;
using System.Xml;

namespace XYS.Lis.Repository.Normal
{
    public class XmlNormalConfigurator
    {
        private Hashtable _sectionTable;
        private const string CONFIGURATION_TAG = "lis";
        private const string SECTIONS_TAG = "sections";
        private const string SECTION_TAG = "section";
        private const string ITEM_TAG = "item";
        private const string NAME_ATTR = "name";
        private const string DISPLAY_ATTR = "display";
        public void Configure(XmlElement element)
        {
            if (this._sectionTable == null)
            {
                return;
            }
            string rootElementName = element.LocalName;
            if (rootElementName != CONFIGURATION_TAG)
            {
                //根节点错误
            }
            foreach(XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    if (currentElement.LocalName == SECTIONS_TAG)
                    {
                        //
                    }
                }
            }
        }

    }
}
