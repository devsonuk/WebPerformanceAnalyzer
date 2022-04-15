using System;
using System.Collections.Generic;
using WebPerformanceAnalyzerWorker.Enums;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class Report : BaseEntity
    {
        public string TaskId { get; set; }
        public string RequestedUrl { get; set; }
        public string FinalUrl { get; set; }
        public string GatherMode { get; set; }
        public string UserAgent { get; set; }
        public List<Audit> Audits { get; set; }
        public List<Category> Categories { get; set; }
        public List<CategoryGroup> CategoryGroups { get; set; }
        public List<Timing> Timings { get; set; }
        public Environment Environment { get; set; }
        public string MetaData { get; set; }
        public int Performance { get; set; }
        public int Accessibility { get; set; }
        public int BestPractices { get; set; }
        public int SEO { get; set; }
        public int PWA { get; set; }
        public double? BenchmarkIndex { get; set; }
        public DateTime FetchTime { get; set; }
        public string HtmlReport { get; set; }
        public Device Device { get; set; }
    }
}
