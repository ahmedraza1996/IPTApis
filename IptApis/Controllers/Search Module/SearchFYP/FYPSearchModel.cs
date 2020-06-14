using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Controllers.Search_Module.SearchFYP
{
    public class FYPSearchModel
    {
        public String supervisor { get; set; }
        public String member1 { get; set; }
        public String member2 { get; set; }
        public String member3 { get; set; }
        public String title { get; set; }
        public String description { get; set; }
        
        public FYPSearchModel(String supervisor, String member1, String member2, String member3, String title, String description)
        {
            this.supervisor = supervisor;
            this.member1 = member1;
            this.member2 = member2;
            this.member3 = member3;
            this.title = title;
            this.description = description;
        }

        public FYPSearchModel() { }

        public IDictionary<string, object> cast()
        {
            Dictionary<string, object> pairs = new Dictionary<string, object>();
            pairs["supervisor"] = supervisor;
            pairs["member1"] = member1;
            pairs["member2"] = member2;
            pairs["member3"] = member3;
            pairs["title"] = title;
            pairs["description"] = description;
            return pairs;
        }
    }
}