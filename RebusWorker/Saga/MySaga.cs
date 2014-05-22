using System;
using Rebus;

namespace RebusWorker.Saga
{
   /// <summary>
   /// Definition of the process
   /// -Which message starts the process
   /// -Which messages is handled by the process
   /// </summary>
    public class MySaga : Saga<MySagaData>,
        IAmInitiatedBy<StartTheSagaMessage>,
        IHandleMessages<Step1FinishedMessage>,
        IHandleMessages<Step2FinishedMessage>
    {
        public IBus Bus { get; set; }
        public void Handle(StartTheSagaMessage message)
        {
            //The saga is started - Do some stuff - call webservices (in external handler)
            //When this step is finished the external process replies with a "step1FinishedMessage"
            this.Data.ProcessId = message.ProcessId;
            //Fake Step1FinishMessage (should be replied from external handler)
            Bus.Send(new Step1FinishedMessage() { ProcessId = this.Data.ProcessId });

        }

        public override void ConfigureHowToFindSaga()
        {
            Incoming<StartTheSagaMessage>(m => m.ProcessId).CorrelatesWith(s => s.ProcessId);
            Incoming<Step1FinishedMessage>(m => m.ProcessId).CorrelatesWith(s => s.ProcessId);
            Incoming<Step2FinishedMessage>(m => m.ProcessId).CorrelatesWith(s => s.ProcessId);
        }

        public void Handle(Step1FinishedMessage message)
        {
            //Sagabehaviour when the Step1 is finished by the external handler
            this.Data.Step1Finished = true;
            //After dalying 10 seconds - Send a step2finishedmessage
            Bus.Defer(TimeSpan.FromSeconds(10), new Step2FinishedMessage() { ProcessId = this.Data.ProcessId });
        }

        public void Handle(Step2FinishedMessage message)
        {
            //Step2 is handled - finished the saga
            this.Data.Step2Finished = true;
            this.MarkAsComplete();
        }
    }
}