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
    public class ReviewDeco: FileDeco<ReviewModel>
    {

        public ReviewDeco(EnglishFeedFile reviewFeedBase, string hostPre, string lang) : base(reviewFeedBase, hostPre, lang)
        {

            Link = $"https://{hostPre}.trip.com/hotels/detail?hotelid=(PARTNER-HOTEL-ID)&language={lang}&checkin=(CHECKINYEAR)-(CHECKINMONTH)-(CHECKINDAY)&checkout=(CHECKOUTYEAR)-(CHECKOUTMONTH)-(CHECKOUTDAY)&curr=(USER-CURRENCY)&Allianceid=15214&Sid=1394408&ouid=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)&utm_medium=cpc&utm_campaign=HPA&utm_source=google&utm_content=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)";

        }

        public override ReviewListingsType GetReviewList()
        {
            return ReviewFeed.GetReviewList();
        }

        public override void FeedXmlProcess(IList<ReviewModel> reviewList)
        {
            var watch = new Stopwatch();
            watch.Start();

            foreach (var d in reviewList)
            {
                //var node = ReviewFeed.GetReviewList().ListModel.FirstOrDefault(r => r.Ctriphotelid == d.HotelID);
                //if (node == null) continue;

                var index = ReviewFeed.SortHotelList.BinarySearchIndex(d.HotelID);
                if (index == -1 || string.IsNullOrWhiteSpace(d.writingcontent)) continue;
                var node = ReviewFeed.GetReviewList().ListModel[index];

                if (node.Content == null)
                {
                    node.Content = new ContentType();
                }

                if (node.Content.ReviewList == null)
                {
                    node.Content.ReviewList = new List<ReviewType>();
                }

                DateTime dt = d.writingdate.ToDateTime();
                node.HasReview = true;
                node.Content.ReviewList.Add(new ReviewType()
                {

                    Link = Link,
                    Author = "Trip.com Member",
                    Title = "Hotel review",
                    Rating = d.ratingall.ToInt()+5,
                    Body = d.writingcontent,
                    Date = new DateType()
                    {
                        Day = dt.Day,
                        Month = dt.Month,
                        Year = dt.Year
                    }

                });
            }

            watch.Stop();
            var st = watch.ElapsedMilliseconds;
        }

    }
}