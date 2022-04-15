using System;
using System.Collections.Generic;
using System.Text;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class Environment
    {
        public string NetworkUserAgent { get; set; }
        public string HostUserAgent { get; set; }
        public double? BenchmarkIndex { get; set; }
        public dynamic Credits { get; set; }
    }
}
