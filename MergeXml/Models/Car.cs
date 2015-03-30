namespace MergeXml.Models
{
    public class Car
    {
        [MapToXmlElement("Make")]
        public string Make { get; set; }

        [MapToXmlElement("Model")]
        public string Model { get; set; }

        [MapToXmlElement("Year")]
        public string Year { get; set; }

        public decimal ListPrice { get; set; }
    }
}
