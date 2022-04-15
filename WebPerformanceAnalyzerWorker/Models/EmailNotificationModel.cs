using System;
using System.Collections.Generic;
using System.IO;
using WebPerformanceAnalyzerWorker.Entities;
using WebPerformanceAnalyzerWorker.Enums;

namespace WebPerformanceAnalyzerWorker.Models
{

    /// <summary>
    /// Represents Email notification model
    /// </summary>
    public class EmailNotificationModel
    {
        public string Url { get; set; }
        public string Device { get; set; }
        public string FetchTime { get; set; }
        public int NumberOfRuns { get; set; }
        public int Performance { get; set; }
        public int Accessibility { get; set; }
        public int BestPractices { get; set; }
        public int SEO { get; set; }
        public int PWA { get; set; }
        public User User { get; set; }
        public double? BenchmarkIndex { get; set; }
        public string RedirectLink { get; set; }
        public string AttachmentName { get; set; }
        public MemoryStream AttachmentStream { get; set; }
        public Dictionary<LastReportType, DeltaChange> DeltaChanges { get; set; }
    }
}
