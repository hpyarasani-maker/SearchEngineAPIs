using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEngineAPIs.Models
{
    public class locationError
    {
        public class Errors
        {
            public int code { get; set; }
            public string context { get; set; }
            public string message { get; set; }
        }

        public class Meta
        {
            public int status { get; set; }
            public Errors errors { get; set; }
        }

        public class RootObject
        {
            public Meta meta { get; set; }
        }
    }
}