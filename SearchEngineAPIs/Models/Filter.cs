using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEngineAPIs.Models
{
    public class Filter
    {
        public int Seid { get; set; }
        public string Source { get; set; }
        public string market { get; set; }
        public string language { get; set; }
        public string Device { get; set; }
        public string Location { get; set; }
        public string Dummy { get; set; }
        public string type { get; set; }
        public string format { get; set; }

    }
}