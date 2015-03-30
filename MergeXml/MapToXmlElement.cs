using System;

namespace MergeXml
{
    /// <summary>
    /// A custom attribute for mapping object properties to XML elements.
    /// </summary>
    public class MapToXmlElement : Attribute
    {
        /// <summary>
        /// The verbatim name of the element in the XML file to overlay
        /// </summary>
        public string ElementName { get; set; }

        public string ChildElementName { get; set; }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="elementName">Verbatim XML element name (case-sensitive).</param>
        public MapToXmlElement(string elementName)
        {
            ElementName = elementName;
        }

        public MapToXmlElement(string elementName, string childElementName) : this(elementName)
        {
            ChildElementName = childElementName;
        }
    }
}