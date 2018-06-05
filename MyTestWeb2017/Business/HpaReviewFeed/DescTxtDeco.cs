using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using MyTestWeb2017.framework;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    public class DescTxtDeco: FileDeco<string>
    {
        public DescTxtDeco(EnglishFeedFile reviewFeedBase, string hostPre, string lang) : base(reviewFeedBase, hostPre, lang)
        {
            Link = $"https://{hostPre}.trip.com/hotels/detail?hotelid=(PARTNER-HOTEL-ID)&language={lang}&checkin=(CHECKINYEAR)-(CHECKINMONTH)-(CHECKINDAY)&checkout=(CHECKOUTYEAR)-(CHECKOUTMONTH)-(CHECKOUTDAY)&curr=(USER-CURRENCY)&Allianceid=15214&Sid=1394411&ouid=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)&utm_medium=cpc&utm_campaign=HPA&utm_source=google&utm_content=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)";
        }

        public override ReviewListingsType GetReviewList()
        {
            return ReviewFeed.GetReviewList();
        }

        public override void FeedXmlProcess(IList<string> descStringList)
        {
            if (descStringList != null)
            {
                var descList = Convert(descStringList);

                for (var i = 0; i < descList.Count; i++)
                {
                    var index = ReviewFeed.SortHotelList.BinarySearchIndex(descList[i].HotelID);
                    if (index == -1 || string.IsNullOrWhiteSpace(descList[i].HotelDescription)) continue;
                    var node = ReviewFeed.GetReviewList().ListModel[index];

                    if (node.Content == null)
                    {
                        node.Content = new ContentType();
                    }

                    node.Content.Text = new TextType()
                    {
                        Link = Link,
                        Body = descList[i].HotelDescription
                    };
                }
            }
        }

        private IList<DescModel> Convert(IList<string> descStringList)
        {
            List<DescModel> descList = new List<DescModel>();
            if (descStringList != null && descStringList.Count > 0)
            {
                descList = new List<DescModel>();
                for (int i = 0; i < descStringList.Count; i++)
                {
                    if (descStringList[i] != null)
                    {
                        var splitIndex = descStringList[i].IndexOfAny(new[]{ ' ', '\t' });
                        int hotel;
                        if (splitIndex > 0 && int.TryParse(descStringList[i].Substring(0, splitIndex), out hotel))
                        {

                            descList.Add(new DescModel()
                            {
                                HotelID = hotel,
                                HotelDescription = descStringList[i].Substring(splitIndex + 1).Trim('\"')
                            });
                        }
                    }
                    
                }

            }

            return descList;
        }
    }
}