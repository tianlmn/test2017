using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    public class DescDeco:FileDeco<DescModel>
    {

        public DescDeco(ReviewListingsType reviewFeedBase, string hostPre, string lang, string locale)
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

        public override void FeedXmlProcess(IList<DescModel> descList)
        {
            foreach (var d in descList)
            {
                var node = ReviewFeed.ListModel.FirstOrDefault(r => r.Ctriphotelid == d.hotelid);
                if (node == null) continue;
                if (node.Content == null)
                {
                    node.Content = new ContentType();
                }
                
                node.Content.Text = new TextType()
                {
                    Link = $"https://{HostPre}.trip.com/hotels/detail?language={Language}&locale={Locale}&hotelid={node.Ctriphotelid}&allianceid=15214&sid=10000",
                    Body = d.desc
                };
            }
        }

    }
}