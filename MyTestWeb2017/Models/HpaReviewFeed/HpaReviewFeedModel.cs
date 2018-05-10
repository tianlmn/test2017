using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyTestWeb2017.Models.HpaReviewFeed
{
    [XmlRoot("listings")]
    public class ReviewListingsType
    {
        [XmlElement("language")]
        public string Language = "en";

        [XmlElement("datum")]
        public string Datum = "WGS84";

        [XmlElement("listing")]
        public List<ReviewListType> ListModel = new List<ReviewListType>();
    }

    [XmlType]
    public class ReviewListType : ListType
    {
        [XmlElement("content", Order = 9)]
        public ContentType Content { get; set; }

        public bool HasReview { get; set; }

        public bool HasName { get; set; }
    }

    [XmlType]
    public class ContentType
    {
        [XmlElement("component")]
        public TextType Text { get; set; }

        [XmlElement("review")]
        public List<ReviewType> ReviewList { get; set; }

        [XmlElement("image")]
        public List<ImageType> ImageList { get; set; }
    }

    [XmlType]
    public class TextType
    {
        [XmlAttribute("type")]
        public string Type = "description";

        [XmlElement("link", Order = 1)]
        public string Link { get; set; }

        [XmlElement("title", Order = 2)]
        public string Title = "Hotel Description";

        [XmlElement("body", Order = 3)]
        public string Body { get; set; }
    }


    [XmlType]
    public class ReviewType
    {
        [XmlAttribute("type")]
        public string Type = "user";

        [XmlElement("link", Order = 1)]
        public string Link { get; set; }

        [XmlElement("title", Order = 2)]
        public string Title { get; set; }

        [XmlElement("author", Order = 3)]
        public string Author { get; set; }

        [XmlElement("rating", Order = 4)]
        public int Rating { get; set; }

        [XmlElement("body", Order = 5)]
        public string Body { get; set; }

        [XmlElement("date", Order = 6)]
        public DateType Date { get; set; }
    }

    [XmlType]
    public class DateType
    {
        [XmlAttribute("day")]
        public int Day{ get; set; }

        [XmlAttribute("month")]
        public int Month { get; set; }

        [XmlAttribute("year")]
        public int Year { get; set; }

    }

    [XmlType]
    public class ImageType
    {
        [XmlAttribute("type")]
        public string Type = "photo";

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlElement("link", Order = 1)]
        public string Link { get; set; }

        [XmlElement("title", Order = 2)]
        public string Title { get; set; }
        
    }




}