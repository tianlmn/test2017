using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyTestWeb2017.Models
{
    [XmlRoot("QueryControl")]
    public class QueryControl
    {
        [XmlElement("ItineraryCapabilities")]
        public  ItineraryCapabilities Itiner { get; set; }
    }

    [XmlType]
    public class ItineraryCapabilities
    {
        [XmlElement("DefaultValue")]
        public DefaultValue Default { get; set; }

        [XmlElement("PropertyOverride")]
        public List<PropertyOverride> Override { get; set; }
    }

    [XmlType]
    public class PropertyOverride
    {
        [XmlElement("MaxAdvancePurchase")]
        public int MaxAdvancePurchase { get; set; }
        [XmlElement("MaxLengthOfStay")]
        public int MaxLengthOfStay { get; set; }
        [XmlElement("State")]
        public string State { get; set; }
        [XmlElement("Property")]
        public List<int> PropertyList{ get; set; }
    }

    [XmlType]
    public class DefaultValue
    {
        [XmlElement("MaxAdvancePurchase")]
        public int MaxAdvancePurchase { get; set; }
        [XmlElement("MaxLengthOfStay")]
        public int MaxLengthOfStay { get; set; }
        [XmlElement("State")]
        public string State { get; set; }

    }
}