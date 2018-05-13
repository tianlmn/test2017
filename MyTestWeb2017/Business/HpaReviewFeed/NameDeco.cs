using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    public class NameDeco : FileDeco<NameModel>
    {

        public NameDeco(ReviewListingsType reviewFeedBase, string hostPre, string lang, string locale)
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

        public override void FeedXmlProcess(IList<NameModel> nameList)
        {
            foreach (var d in nameList)
            {
                var node = ReviewFeed.ListModel.FirstOrDefault(r => r.Ctriphotelid == d.HotelId);
                if (node == null) continue;

                node.HasName = true;
                if (!string.IsNullOrWhiteSpace(d.HotelName))
                {
                    node.HotelName = new Models.HotelNameType()
                    {
                        Language = Language,
                        Value = d.HotelName
                    };
                }

                if (node.Addresscn == null)
                    node.Addresscn = new Models.AddressType();

                if (!string.IsNullOrWhiteSpace(d.Address))
                {
                    var address = node.Addresscn.Component.FirstOrDefault(ad => ad.Name == "addr1");
                    if (address != null) address.Value = d.Address;
                }

                if (!string.IsNullOrWhiteSpace(d.cityname))
                {
                    var city = node.Addresscn.Component.FirstOrDefault(ad => ad.Name == "city");
                    if (city != null) city.Value = d.cityname;
                }

                if (!string.IsNullOrWhiteSpace(d.provincename))
                {
                    var province = node.Addresscn.Component.FirstOrDefault(ad => ad.Name == "province");
                    if (province != null) province.Value = d.provincename;
                }


            }
        }
    }
}