using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTestWeb2017.Models;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    public class EnglishFeedFile:IReviewFeedBase<HotelFeedModel>
    {
        private ReviewListingsType ReviewList { get; set; }

        public int[] SortHotelList { get; set; }

        public EnglishFeedFile()
        {
            ReviewList = new ReviewListingsType()
            {
                ListModel = new List<ReviewListType>()
            };

        }

        public void FeedXmlProcess(IList<HotelFeedModel> dataList)
        {
            foreach (var d in dataList)
            {
                try
                {
                    int hotel;
                    if (d != null && int.TryParse(d.Ctriphotelid, out hotel))
                    {
                        var t = new ReviewListType()
                        {
                            Ctriphotelid = hotel,
                            HotelName = new HotelNameType() { Value = d.hotelname },
                            Addresscn = new AddressType()
                            {
                                Component = new List<Component>()
                                {
                                    new Component(){Name = "addr1", Value=d.address},
                                    new Component(){Name = "province", Value=d.provincename},
                                    new Component(){Name = "city", Value=d.cityname},
                                    new Component(){Name = "postal_code",Value=d.address_postcode},
                                }
                            },
                            Country = d.countrycode,
                            Lat = Convert.ToDouble(d.latitude),
                            Lon = Convert.ToDouble(d.longitude),
                            Category = "Hotel",
                            Phone = new PhoneType() { Value = d.phone }
                        };
                        ReviewList.ListModel.Add(t);
                    }
                }
                catch
                {

                }
                
            }
        }

        public ReviewListingsType GetReviewList()
        {
            return ReviewList ?? (ReviewList = new ReviewListingsType());
        }

        public void ConvertList()
        {

        }

        public void FeedXmlProcess()
        {
            throw new NotImplementedException();
        }
    }
}