using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEngineAPIs.Models
{
    public class Locations
    {
        public class Data
        {
            public string market { get; set; }
            public string id { get; set; }
            public string source { get; set; }
            public string language { get; set; }
            public string device { get; set; }
            public List<string> message { get; set; }
            public searchengine Searchengine { get; set; }
            public searchengines Searchengines { get; set; }
            public List<string> location { get; set; }
            public List<string> locations { get; set; }

        }

        public class searchengine
        {
            public List<string> id { get; set; }
            public string market { get; set; }
            public string source { get; set; }
            public string language { get; set; }
            public string device { get; set; }
            public List<string> location { get; set; }
        }
        public class searchengines
        {
            public List<string> id { get; set; }
            public string market { get; set; }
            public string source { get; set; }
            public string language { get; set; }
            public string device { get; set; }
            public List<string> location { get; set; }
        }
        public class Meta
        {
            public int status { get; set; }
            public Data data { get; set; }

            public searchengine searchengine { get; set; }

            public searchengines searchengines { get; set; }
        }

        public class RootObject
        {
            public Meta meta { get; set; }
        }
    }
}