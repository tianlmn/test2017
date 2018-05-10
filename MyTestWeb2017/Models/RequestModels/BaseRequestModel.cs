using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyTestWeb2017.mvc;

namespace MyTestWeb2017.Models.RequestModels
{
    public class BaseRequestModel
    {
        [BindAlias("city")]
        public int CityId { get; set; }

        [BindAlias("hotel")]
        public int HotelId { get; set; }

        [BindAlias("room")]
        public int RoomId { get; set; }

        public DateTime CheckIn { get; set; }

        public string Words { get; set; }

        public DateTime CheckOut { get; set; }

        public virtual bool Validate()
        {
            return true;
        }

        public MvcHtmlString MHString { get; set; }
    }
}