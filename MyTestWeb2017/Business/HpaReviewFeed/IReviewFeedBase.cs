using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTestWeb2017.Models;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    
    public interface IReviewFeedBase<T>
    {
        ReviewListingsType GetReviewList();

        void ConvertList();

        void FeedXmlProcess(IList<T> t);
    }


}