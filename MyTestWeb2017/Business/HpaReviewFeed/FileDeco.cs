using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTestWeb2017.Models;
using MyTestWeb2017.Models.HpaReviewFeed;

namespace MyTestWeb2017.Business.HpaReviewFeed
{
    public class FileDeco<T>: IReviewFeedBase<T>
    {
        public FileDeco(EnglishFeedFile reviewFeedBase, string hostPre, string lang) {
            ReviewFeed = reviewFeedBase;
            HostPre = hostPre;
            Language = lang;
        }

        public IList<T> TargetListing { get; set; }

        protected EnglishFeedFile ReviewFeed { get; set; }

        protected string HostPre { get; set; }

        protected string Language { get; set; }

        protected string Link { get; set; }

        public virtual ReviewListingsType GetReviewList()
        {
            return null;
        }

        public void ConvertList()
        {
            throw new NotImplementedException();
        }

        public virtual void FeedXmlProcess(IList<T> t)
        {
            throw new NotImplementedException();
        }
    }
}