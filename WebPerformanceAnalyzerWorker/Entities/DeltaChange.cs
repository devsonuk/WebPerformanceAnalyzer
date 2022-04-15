using System;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class DeltaChange : BaseEntity
    {
        public int Performance { get; set; }
        public int Accessibility { get; set; }
        public int BestPractices { get; set; }
        public int SEO { get; set; }
        public int PWA { get; set; }
        public DateTime FetchTime { get; set; }
    }
}
