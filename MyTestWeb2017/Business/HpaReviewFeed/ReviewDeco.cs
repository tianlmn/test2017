using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTestWeb2017.framework;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    public class ReviewDeco: FileDeco<ReviewModel>
    {

        public ReviewDeco(ReviewListingsType reviewFeedBase, string hostPre, string lang, string locale)
        {
            ReviewFeed = reviewFeedBase;
            HostPre = hostPre;
            Language = lang;
            Locale = locale;

        }

        public override ReviewListingsType GetReviewList()
        {
            return ReviewFeed;
        }

        public override void FeedXmlProcess(IList<ReviewModel> reviewList)
        {
            foreach (var d in reviewList)
            {
                var node = ReviewFeed.ListModel.FirstOrDefault(r => r.Ctriphotelid == d.HotelID);
                if (node == null) continue;
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

                    Link = $"https://{HostPre}.trip.com/hotels/detail?language={Language}&locale={Locale}&hotelid={node.Ctriphotelid}&allianceid=15214&sid=10000",
                    Author = "Trip.com Member",
                    Title = "Hotel review",
                    Rating = d.ratingall.ToInt()*2,
                    Body = d.writingcontent,
                    Date = new DateType()
                    {
                        Day = dt.Day,
                        Month = dt.Month,
                        Year = dt.Year
                    }

                });
            }
        }

    }
}