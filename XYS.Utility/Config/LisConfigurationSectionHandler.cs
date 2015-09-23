using System.Xml;
using System.Configuration;

namespace XYS.Utility.Config
{
    public class LisConfigurationSectionHandler : IConfigurationSectionHandler
    {

        public LisConfigurationSectionHandler()
        {
 
        }
        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }
    }
}
