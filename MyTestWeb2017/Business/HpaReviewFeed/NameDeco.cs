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
    public class NameDeco : FileDeco<NameModel>
    {

        public NameDeco(EnglishFeedFile reviewFeedBase, string hostPre, string lang) : base(reviewFeedBase, hostPre, lang)
        {
        }

        public override ReviewListingsType GetReviewList()
        {
            return ReviewFeed.GetReviewList();
        }

        public override void FeedXmlProcess(IList<NameModel> nameList)
        {
            var watch = new Stopwatch();
            watch.Start();

            foreach (var d in nameList)
            {
                //var node = ReviewFeed.GetReviewList().ListModel.FirstOrDefault(r => r.Ctriphotelid == d.HotelId);
                //if (node == null) continue;

                var index = ReviewFeed.SortHotelList.BinarySearchIndex(d.HotelId);
                if (index == -1) continue;
                var node = ReviewFeed.GetReviewList().ListModel[index];

                node.HasName = true;
                if (!string.IsNullOrWhiteSpace(d.HotelName))
                {
                    node.HotelName.Language = Language;
                    node.HotelName.Value = d.HotelName;
                }


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

            watch.Stop();
            var st = watch.ElapsedMilliseconds;
        }
    }
}