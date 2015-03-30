using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Xml.Linq;
using MergeXml.Models;

namespace MergeXml.Tests
{
    [TestFixture]
    public class MergeTests
    {
        [Test]
        public void Test_merge_config_obj()
        {
            const string testXml =
                @"
                <settings>
                    <someId>1</someId>
                    <someName>Original Value</someName>
                    <someList>
                        <string>testing</string>
                    </someList>
                    <someInt>5000</someInt>
                    <undecoratedInt>10000</undecoratedInt>
                </settings>";

            var testConfig = new Config
            {
                Id = Guid.NewGuid(),
                Name = null,  // name element should be empty after merge as the property value is null
                FirstList = new List<string> {"just", "a", "test"},
                SecondList = new List<string> {"another", "test"},
                SomeIntValue = 4500,
                UndecoratedProperty = 5000 // should not be merged as the property is not decorated with a mapping attribute
            };

            var mergedXml = XmlMapper.Merge<Config>(testXml, testConfig);

            var root = XDocument.Parse(mergedXml).Descendants("settings").FirstOrDefault();

            Assert.IsNotNull(root);
            Assert.That(root.Element("someId").Value, Is.EqualTo(testConfig.Id.ToString()));
            Assert.IsTrue(root.Element("someName").IsEmpty);
            Assert.IsTrue(root.Element("someList").HasElements);
            Assert.That(root.Element("someList").Descendants("string").Count(), Is.EqualTo(5));
            Assert.That(root.Element("someInt").Value, Is.EqualTo(testConfig.SomeIntValue.ToString()));
            Assert.That(root.Element("undecoratedInt").Value, Is.EqualTo("10000"), "Property not decorated to map to element, so value should not have been changed.");
        }

        [Test]
        public void Test_merge_car_obj()
        {
            const string testXml =
                @"
                <Car>
                    <Make>Honda</Make>
                    <Model>Civic</Model>
                    <Year>2008</Year> 
                    <Price>18000</Price>   
                </Car>";

            var newCar = new Car
            {
                Make = "Ford",
                Model = "Focus",
                Year = "2010",
                ListPrice = 15000
            };

            var mergedXml = XmlMapper.Merge<Car>(testXml, newCar);

            var root = XDocument.Parse(mergedXml).Descendants("Car").FirstOrDefault();

            Assert.IsNotNull(root);
            Assert.That(root.Element("Make").Value, Is.EqualTo(newCar.Make));
            Assert.That(root.Element("Model").Value, Is.EqualTo(newCar.Model));
            Assert.That(root.Element("Year").Value, Is.EqualTo(newCar.Year));
            Assert.That(root.Element("Price").Value, Is.EqualTo("18000"), "Property not decorated to map to element, so value should not have been changed.");
        }
    }
}