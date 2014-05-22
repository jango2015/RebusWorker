using Rebus;
using Rebus.Logging;

namespace RebusWorker.MessageHandlers
{
    public class HandleTestEvent : IHandleMessages<TestEvent>
    {
        public IBus Bus { get; set; }
        private static ILog log;

        static HandleTestEvent()
        {
            RebusLoggerFactory.Changed += f => log = f.GetCurrentClassLogger();
        }
        public void Handle(TestEvent message)
        {
            log.Debug("Got event" + message.Message);
        }
    }
}