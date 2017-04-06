using System.Xml;

namespace ComicPresence.Common.Xml
{
    public static class XmlElementExtensions
    {
        public static void AddTextElement(this XmlElement elem, string newElemName, string text)
        {
            XmlElement newElem = elem.OwnerDocument.CreateElement(newElemName);
            newElem.InnerText = text;
            elem.AppendChild(newElem);
        }
    }
}
