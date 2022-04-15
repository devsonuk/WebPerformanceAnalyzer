using System;
using System.Collections.Generic;
using System.Text;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class Timing
    {
        public double? StartTime { get; set; }
        public string Name { get; set; }
        public double? Duration { get; set; }
        public string EntryType { get; set; }
    }
}
