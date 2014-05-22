using System;

namespace RebusWorker.Saga
{
    public class StartTheSagaMessage
    {

        public Guid ProcessId { get; set; }
    }
}