using Rebus;
using Rebus.Logging;
using RebusWorker.Message;

namespace RebusWorker.MessageHandlers
{
    public class HandleTestMessage2 : IHandleMessages<TestMessage2>
    {
        public IBus Bus { get; set; }
        private static ILog log;

        static HandleTestMessage2()
        {
            RebusLoggerFactory.Changed += f => log = f.GetCurrentClassLogger();
        }
        public void Handle(TestMessage2 message)
        {
            log.Info("Handler2 - got message: {0}", message.Message);
        }
    }
}