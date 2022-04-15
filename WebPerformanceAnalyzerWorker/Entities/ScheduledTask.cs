using System;
using WebPerformanceAnalyzerWorker.Enums;

namespace WebPerformanceAnalyzerWorker.Entities
{
    public class ScheduledTask : BaseEntity
    {
        public string Domain { get; set; }
        public string Url { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string UserId { get; set; }
        public Priority Priority { get; set; }
        public bool IsActive { get; set; }
        public ScheduledTaskType ScheduledTaskType { get; set; }
    }
}
