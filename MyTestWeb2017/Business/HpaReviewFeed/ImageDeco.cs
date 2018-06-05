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
    public class ImageDeco: FileDeco<ImageModel>
    {

        public ImageDeco(EnglishFeedFile reviewFeedBase, string hostPre, string lang) : base(reviewFeedBase, hostPre, lang)
        {
            Link = $"https://{hostPre}.trip.com/hotels/detail?hotelid=(PARTNER-HOTEL-ID)&language={lang}&checkin=(CHECKINYEAR)-(CHECKINMONTH)-(CHECKINDAY)&checkout=(CHECKOUTYEAR)-(CHECKOUTMONTH)-(CHECKOUTDAY)&curr=(USER-CURRENCY)&Allianceid=15214&Sid=1394410&ouid=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)&utm_medium=cpc&utm_campaign=HPA&utm_source=google&utm_content=(CHECKINDAY)_(CHECKINMONTH)_(CHECKINYEAR)_(LENGTH)_(GOOGLE-SITE)_(PARTNER-HOTEL-ID)_(USER-COUNTRY)_(USER-DEVICE)_(DATE-TYPE)";

        }

        public override ReviewListingsType GetReviewList()
        {
            return ReviewFeed.GetReviewList();
        }

        public override void FeedXmlProcess(IList<ImageModel> imageList)
        {
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < imageList.Count; i++)
            {
                //var node = ReviewFeed.GetReviewList().ListModel.Find(r => r.Ctriphotelid == imageList[i].hotelid);

                var index = ReviewFeed.SortHotelList.BinarySearchIndex(imageList[i].hotelid);
                if (index == -1 || string.IsNullOrWhiteSpace(imageList[i].URL)) continue;
                var node = ReviewFeed.GetReviewList().ListModel[index];

                if (node.Content == null)
                {
                    node.Content = new ContentType();
                }

                if (node.Content.ImageList == null)
                {
                    node.Content.ImageList = new List<ImageType>();
                }

                node.Content.ImageList.Add(new ImageType()
                {
                    Url = imageList[i].URL,
                    Link = Link,
                    Title = imageList[i].typename
                });
            }

            watch.Stop();
            var st = watch.ElapsedMilliseconds;
        }

    }
}