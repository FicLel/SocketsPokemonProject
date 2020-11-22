using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlWorker
{
    public class XmlConverter
    {
        public String[] GetXmlData(String xmlWithOutFormatting)
        {
            //This list will be parse to an Array
            List<String> ArrayList = new List<String>();
            //We start transforming our data
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlWithOutFormatting);
            XmlNodeList TypesNode = document.SelectNodes("//Types");
            foreach (XmlNode Type in TypesNode)
            {
                Console.WriteLine(Type.InnerText);
                ArrayList.Add(Type.InnerText);
            }
            return ArrayList.ToArray();
        }
        
        public String ProcessXmlResponse(String[] arrayForResponse, bool isResponse)
        {
            Console.WriteLine("Procesando");
            XmlDocument newXmlDocument = new XmlDocument();
            XmlNode documentNode = newXmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            newXmlDocument.AppendChild(documentNode);
            XmlNode rootNode = isResponse ? newXmlDocument.CreateElement("Response") : newXmlDocument.CreateElement("Request");
            newXmlDocument.AppendChild(rootNode);
            XmlNode typesNode = newXmlDocument.CreateElement("Types");
            rootNode.AppendChild(typesNode);
            XmlNode typeNode;
            foreach (String type in arrayForResponse)
            {
                Console.WriteLine(type);
                typeNode = newXmlDocument.CreateElement("Type");
                typeNode.InnerText = type;
                typesNode.AppendChild(typeNode);
            }
            return newXmlDocument.InnerXml;
        }
    }
}
