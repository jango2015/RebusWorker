using System;

namespace RebusWorker.Saga
{
    public class Step2FinishedMessage
    {
        public Guid ProcessId { get; set; }
    }
}