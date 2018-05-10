using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MyTestWeb2017.Models
{
    public class HotelFeedModel
    {
        public string Ctriphotelid { get; set; }

        public string hotelname { get; set; }

        public string address { get; set; }

        public string countrycode { get; set; }

        public string cityname { get; set; }

        public string provincename { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }

        public string address_postcode { get; set; }

        public string phone { get; set; }


    }




    [XmlRoot("listings")]
    public class ListingsType
    {
        [XmlElement("language")]
        public string Language = "en";

        [XmlElement("datum")]
        public string Datum = "WGS84";

        [XmlElement("listing")]
        public List<ListType> ListModel=new List<ListType>();
    }

    [XmlType]
    public class ListType
    {
        [XmlElement("id",Order =1)]
        public int Ctriphotelid { get; set; }

        [XmlElement("name", Order = 2)]
        public HotelNameType HotelName{ get; set; }

        [XmlElement("address", Order = 3)]
        public AddressType Addresscn { get; set; }

        [XmlElement("country", Order = 4)]
        public string Country{ get; set; }

        [XmlElement("longitude", Order = 6)]
        public double Lon { get; set; }

        [XmlElement("latitude", Order = 5)]
        public double Lat { get; set; }

        [XmlElement("phone", Order = 7)]
        public PhoneType Phone { get; set; }

        [XmlElement("category", Order = 8)]
        public string Category{ get; set; }
    }

    [XmlType]
    public class PhoneType
    {
        [XmlAttribute("type")]
        public string Type = "main";

        [XmlText]
        public string Value { get; set; }
    }

    [XmlType()]
    public class AddressType
    {
        [XmlAttribute("format")]
        public string Format = "simple";

        [XmlAttribute("language")]
        public string Language = "ja";

        [XmlElement("component")]
        public List<Component> Component { get; set; }
    }

    [XmlType]
    public class HotelNameType
    {
        [XmlAttribute("language")]
        public string Language = "ja";

        [XmlText]
        public string Value { get; set; }
    }

    [XmlType]
    public class Component
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlText]
        public string Value { get; set; }
    }
}