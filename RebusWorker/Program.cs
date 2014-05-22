using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Rebus;
using Rebus.Configuration;
using Rebus.Logging;
using Rebus.Transports.Msmq;
using Rebus.Transports.Sql;
using RebusWorker.Message;
using RebusWorker.Saga;
using Topshelf;

namespace RebusWorker
{
    class Program
    {
        private IContainer container;
        static ILog log;

        static void Main(string[] args)
        {

            RebusLoggerFactory.Changed += f => log = f.GetCurrentClassLogger();

            HostFactory
               .Run(c =>
               {
                   c.SetServiceName(typeof(Program).FullName);
                   c.SetDescription("This is a simple rebus workerservice.");

                   c.Service<Program>(s =>
                   {
                       s.ConstructUsing(() => new Program());

                       s.WhenStarted(p => p.Start());
                       s.WhenStopped(p => p.Stop());
                   });

                   c.DependsOnMsmq();
                   c.StartManually();
               });





        }

        void Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(x => x.Namespace != null && x.Namespace.StartsWith("RebusWorker"))
                .AsClosedTypesOf(typeof(IHandleMessages<>)).AsImplementedInterfaces().InstancePerDependency().PropertiesAutowired();

            container = builder.Build();

            var bus = Configure.With(new Rebus.Autofac.AutofacContainerAdapter(container))
                .Logging(l => l.Use(new ConsoleLoggerFactory(true)))
                //.Transport(t => t.UseSqlServerAndGetInputQueueNameFromAppConfig("server=localhost;database=rebus_test;integrated security=SSPI;").EnsureTableIsCreated())
                .Transport(t => t.UseMsmq("RebusWorker.Input", "RebusWorker.Error"))
                .Serialization(s => s.UseJsonSerializer())
                //.MessageOwnership(d => d.FromRebusConfigurationSection())
                .MessageOwnership(d => d.Use(new SendToSelfResolver()))
                .Sagas(s => s.StoreInSqlServer(Connectionstring, "Sagas", "SagaIndexTable").EnsureTablesAreCreated())
                .Timeouts(t => t.StoreInSqlServer(Connectionstring, "Timeouts").EnsureTableIsCreated())
                .Subscriptions(s => s.StoreInSqlServer(Connectionstring, "Subscriptions").EnsureTableIsCreated())
                .CreateBus()
                .Start();

            //Pub-sub
            bus.Subscribe<TestEvent>();
            bus.Publish(new TestEvent { Message = "Testevent" + DateTime.Now });
            
            //Send two simple messages
            bus.Send(new TestMessage { Message = "Message " + DateTime.Now });
            bus.Send(new TestMessage2 { Message = "Message2 " + DateTime.Now });
            
            //Send message in order to start a saga
            bus.Send(new StartTheSagaMessage() { ProcessId = Guid.NewGuid() });

            Console.WriteLine("Startet - Waiting for messages");
            


        }

        private static string Connectionstring
        {
            get { return "server=localhost;database=rebus_test;integrated security=SSPI;"; }
        }

        void Stop()
        {
            container.Dispose();
        }
    }

    public class TestEvent
    {
        public string Message { get; set; }
    }
}
