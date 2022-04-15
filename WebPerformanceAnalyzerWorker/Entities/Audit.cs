using System;
using System.Collections.Generic;
using System.Text;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class Audit
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double? Score { get; set; }
        public string ScoreDisplayMode { get; set; }
        public double? NumericValue { get; set; }
        public string NumericUnit { get; set; }
        public string DisplayValue { get; set; }
        public dynamic Details { get; set; }
    }
}
