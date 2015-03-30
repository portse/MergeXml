using System;
using System.Collections.Generic;

namespace MergeXml.Models
{
    public class Config
    {
        [MapToXmlElement("someId")]
        public Guid Id { get; set; }

        [MapToXmlElement("someName")]
        public string Name { get; set; }

        [MapToXmlElement("someInt")]
        public int SomeIntValue { get; set; }

        [MapToXmlElement("someList", childElementName: "string")]
        public List<string> FirstList { get; set; }

        [MapToXmlElement("someList", childElementName: "string")]
        public List<string> SecondList { get; set; }

        public int UndecoratedProperty { get; set; }
    }
}