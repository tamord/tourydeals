using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTouristBook
{
    public class FeedInfo
    {
        public string kind;
        public int authoraid;
        public string authorusername;
        public DateTime? date;
        public string title;
        public string shortdesc;                
        public string destination;
        public string imageurl;
        public int priority;
        public int sponsored;
        public int featured;
        public int popular;
        public int isnew;
        public int likes;
        public int? threadid;
        public int? dealid;
        public int? blogid;

        public FeedInfo(string kind, int authoraid, DateTime? date, string title, string destination)
        {
            this.kind = kind;
            this.authoraid = authoraid;
            this.date = date;
            this.title = title;
            this.destination = destination;
        }

    }
}