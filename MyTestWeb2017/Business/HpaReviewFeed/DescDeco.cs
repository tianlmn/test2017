using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using MyTestWeb2017.framework;
using MyTestWeb2017.Models;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    public class DescDeco:FileDeco<DescModel>
    {

        public DescDeco(EnglishFeedFile reviewFeedBase, string hostPre, string lang) : base(reviewFeedBase, hostPre, lang)
        {
        }

        public override ReviewListingsType GetReviewList()
        {
            return ReviewFeed.GetReviewList();
        }

        public override void FeedXmlProcess(IList<DescModel> descList)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (var i=0;i<descList.Count;i++)
            {
                //var node = ReviewFeed.ListModel.Find(r => r.Ctriphotelid == descList[i].HotelID);
                //if (node == null) continue;

                var index = ReviewFeed.SortHotelList.BinarySearchIndex(descList[i].HotelID);
                if (index == -1 || string.IsNullOrWhiteSpace(descList[i].HotelDescription)) continue;
                var node = ReviewFeed.GetReviewList().ListModel[index];

                if (node.Content == null)
                {
                    node.Content = new ContentType();
                }

                node.Content.Text = new TextType()
                {
                    Link = $"https://jp.trip.com/hotels/detail?hotelid=(PARTNER-HOTEL-ID)&language=jp&checkin=(CHECKINYEAR)-(CHECKINMONTH)-(CHECKINDAY)&checkout=(CHECKOUTYEAR)-(CHECKOUTMONTH)-(CHECKOUTDAY)&curr=(USER-CURRENCY)&Allianceid=15214&Sid=1394411&ouid=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)&utm_medium=cpc&utm_campaign=HPA&utm_source=google&utm_content=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)",
                    Body = descList[i].HotelDescription
                };
            }

            watch.Stop();
            var ms = watch.ElapsedMilliseconds;
        }

    }
}