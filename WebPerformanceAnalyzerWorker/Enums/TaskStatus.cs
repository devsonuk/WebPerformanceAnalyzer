namespace WebPerformanceAnalyzerWorker.Enums
{
    public enum TaskStatus
    {
        InQueue = 1,
        Pending = 2,
        NotYetScheduled = 3,
        Disabled = 4,
        InProgress = 5,
        Scheduled = 6,
        UnderScheduling = 7,
        Completed = 8,
        Starting = 9,
        Failure = 10,
        CompletedWithErrors = 11,
        Draft = 12,
        Skipped = 13,
        WaitingForReOauth = 14
    }
}
