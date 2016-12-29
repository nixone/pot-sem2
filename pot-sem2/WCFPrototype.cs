using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pot_sem2
{
    class WCFPrototype
    {
        public WCFPrototype()
        {

        }

        public void Run()
        {
            Thread serverThread = new Thread(RunServer);
            serverThread.Start();
        }

        public void RunServer()
        {
            ServiceHost host = new ServiceHost(typeof(SampleService), new Uri[] { new Uri("net.tcp://localhost:8123/sample") });
            try
            {
                host.Open();
                Thread clientThread = new Thread(RunClient);
                clientThread.Start();
                Thread client2Thread = new Thread(RunClient);
                client2Thread.Start();
                clientThread.Join();
                client2Thread.Join();
                host.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("There was some problem! {0}", e.Message);
                host.Abort();
            }
        }

        public void RunClient()
        {
            using (var channelFactory = new ChannelFactory<ISampleService>("pot_sem2.SampleService", new EndpointAddress("net.tcp://localhost:8123/sample")))
            {
                ISampleService service = channelFactory.CreateChannel();

                Console.WriteLine(service.GetHelloMessage("World"));
            }
        }
    }
}
