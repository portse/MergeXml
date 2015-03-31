using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace MergeXml
{
    /// <summary>
    /// Class responsible for merging object property values into existing XML files.
    /// </summary>
    public class XmlMapper
    {
        private static XDocument _xDoc;

        /// <summary>
        /// Merge object property values with elements in an XML file and return the merged XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Merge<T>(string xml, object obj)
        {
            _xDoc = XDocument.Parse(xml);

            var propsWithAttr =
                typeof (T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof (MapToXmlElement))).ToList();

            ClearElements(propsWithAttr);

            foreach (var prop in propsWithAttr)
            {
                var elementAttr =
                    (MapToXmlElement) prop.GetCustomAttributes(typeof (MapToXmlElement), false).FirstOrDefault();
                if (elementAttr == null) continue;

                SetElementValue(elementAttr, prop.GetValue(obj, null));
            }

            return _xDoc.ToString();
        }

        private static void ClearElements(IEnumerable<PropertyInfo> propsWithAttr)
        {
            foreach (var property in propsWithAttr)
            {
                var elementAttr =
                    (MapToXmlElement) property.GetCustomAttributes(typeof (MapToXmlElement), false).FirstOrDefault();
                if (elementAttr == null) continue;

                var propElement = _xDoc.Descendants(elementAttr.ElementName).FirstOrDefault();
                if (propElement == null) continue;

                // start with an empty element
                propElement.ReplaceWith(new XElement(elementAttr.ElementName));
            }
        }

        private static void SetElementValue(MapToXmlElement elementAttribute, object objValue)
        {
            var element = _xDoc.Descendants(elementAttribute.ElementName).FirstOrDefault();
            if (element == null) return;

            var list = objValue as IList;

            if (list != null)
            {
                var mergedList = list.Cast<object>();

                if (element.HasElements)
                {
                    mergedList =
                        element.Descendants(elementAttribute.ChildElementName).Select(x => x.Value).Concat(mergedList);
                }

                var newListElement = new XElement(elementAttribute.ElementName);

                foreach (var item in mergedList)
                {
                    newListElement.Add(new XElement(elementAttribute.ChildElementName, item));
                }

                element.ReplaceWith(newListElement);
                return;
            }

            element.ReplaceWith((objValue == null
                ? new XElement(elementAttribute.ElementName)
                : new XElement(elementAttribute.ElementName, objValue)));
        }
    }
}