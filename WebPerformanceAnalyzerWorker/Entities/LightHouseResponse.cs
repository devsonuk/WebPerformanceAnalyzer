using System;
using System.Collections.Generic;
using System.Text;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class LightHouseResponse
    {
        public JsonData JsonData { get; set; }
        public string HtmlReport { get; set; }

    }

    public class JsonData
    {
        public string LighthouseVersion { get; set; }
        public string RequestedUrl { get; set; }
        public string FinalUrl { get; set; }
        public string GatherMode { get; set; }
        public DateTime FetchTime { get; set; }
        public List<dynamic> RunWarnings { get; set; }
        public string UserAgent { get; set; }
        public Environment Environment { get; set; }
        public dynamic Audits { get; set; }
        public dynamic ConfigSettings { get; set; }
        public dynamic Categories { get; set; }
        public dynamic CategoryGroups { get; set; }
        public dynamic StackPacks { get; set; }
        public dynamic Timing { get; set; }
        public dynamic I18n { get; set; }
    }
}
