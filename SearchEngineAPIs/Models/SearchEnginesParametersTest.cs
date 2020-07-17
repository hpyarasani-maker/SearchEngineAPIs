namespace SearchEngineAPIs.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SearchEnginesParametersTest
    {
        public int Search_engine { get; set; }
        public string Source { get; set; }
        public string market { get; set; }
        public string language { get; set; }
        public string Device { get; set; }
        public string Location { get; set; }
    }
}
