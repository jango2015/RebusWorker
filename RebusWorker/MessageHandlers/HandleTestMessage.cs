using System;
using Rebus;
using Rebus.Logging;
using RebusWorker.Message;

namespace RebusWorker.MessageHandlers
{
    public class HandleTestMessage : IHandleMessages<TestMessage>
    {
        public IBus Bus { get; set; }
        private static ILog log;

        static HandleTestMessage()
        {
            RebusLoggerFactory.Changed += f => log = f.GetCurrentClassLogger();
        }
        public void Handle(TestMessage message)
        {
            log.Info("Handler got message: {0}", message.Message);
        }
    }
}