using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pot_sem2
{
    public class GameHost
    {
        private String address;
        private int port;
        private Thread thread;
        private Game game;
        private Boolean running = false;

        public GameHost(Game game, String address, int port)
        {
            this.address = address;
            this.port = port;
            this.game = game;
            thread = new Thread(Run);
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        public void Run()
        {
            running = true;
            ServiceHost host = new ServiceHost(game, new Uri[] { new Uri("net.tcp://"+address+":"+port+"/game") });
            try
            {
                host.Open();
                Console.WriteLine("Host running");
                while (running)
                {
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Host closing");
                host.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("There was some problem! {0}", e.Message);
                host.Abort();
            }
            running = false;
        }
    }
}
