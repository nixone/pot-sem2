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
        public delegate void StartedHandler();
        public delegate void StoppedHandler();

        public event StartedHandler OnStarted;
        public event StoppedHandler OnStopped;

        private String address;
        private int port;
        private Thread thread;
        private Game game;
        private Boolean running = false;
        private GameService gameService;

        public GameHost(String address, int port)
        {
            this.address = address;
            this.port = port;
            thread = new Thread(Run);
            thread.IsBackground = true;
            gameService = new GameService();
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            running = false;
            thread.Join();
        }

        public void Run()
        {
            running = true;
            ServiceHost host = new ServiceHost(gameService, new Uri[] { new Uri("net.tcp://"+address+":"+port+"/game") });
            try
            {
                host.Open();
                if (OnStarted != null)
                {
                    OnStarted();
                }
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
            if (OnStopped != null)
            {
                OnStopped();
            }
            running = false;
        }
    }
}
