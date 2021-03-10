using System.Text;
using System.Xml;
using System.Xml.Linq;

public class QuickXMLpublic
{
      /// <summary>
      /// 创建xml
      /// </summary>
      /// <param name="xElement"></param>
      /// <param name="xmlName"></param>
      public static void CreateXml(XElement xElement,string xmlName)
      {
            if(xElement == null || string.IsNullOrEmpty(xmlName))
            {
                  return;
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(CommonLoadPath.ServerXmlPath, settings);
            xElement.Save(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();
      }
}