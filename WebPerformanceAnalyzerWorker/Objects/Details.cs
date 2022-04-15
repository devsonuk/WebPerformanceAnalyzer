using System;
using System.Collections.Generic;
using System.Text;

namespace WebPerformanceAnalyzerWorker.Objects
{
    public sealed class Details
    {
        public string Name { get; set; }
        public decimal? Score { get; set; }
        public decimal? NumericValue {get; set;}
    }
}
