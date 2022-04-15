using System;
using System.Collections.Generic;
using System.Text;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class Category
    {
        public string Title { get; set; }
        public List<string> SupportedModes { get; set; }
        public List<AuditRef> AuditRefs { get; set; }
        public string Id { get; set; }
        public double? Score { get; set; }
    }

    public class AuditRef
    {
        public string Id { get; set; }
        public int Weight { get; set; }
        public string Group { get; set; }
        public string Acronym { get; set; }
        public List<string> RelevantAudits { get; set; }
    }
}
