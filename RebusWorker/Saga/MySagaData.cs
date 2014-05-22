using System;
using Rebus;

namespace RebusWorker.Saga
{
    /// <summary>
    /// Class used to store processdata collected during the saga
    /// Typically statuses, id's etc used to track progress
    /// </summary>
    public class MySagaData : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }
        public Guid ProcessId { get; set; }

        public bool Step1Finished { get; set; }
        public bool Step2Finished { get; set; }
        
    }
}