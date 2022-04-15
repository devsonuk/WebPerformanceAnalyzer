using System;
using System.Collections.Generic;
using System.Text;

namespace WebPerformanceAnalyzerWorker.Objects
{
    public sealed class TimingDetails
    {
        public string Name { get; set; }
        public decimal? StartTime { get; set; }
        public decimal? Duration { get; set; }
    }
}
