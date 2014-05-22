using System;

namespace RebusWorker.Saga
{
    public class Step1FinishedMessage
    {
        public Guid ProcessId { get; set; }
    }
}