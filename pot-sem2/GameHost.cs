using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pot_sem2
{
    /// <summary>
    /// Representation of game host, without client itself
    /// </summary>
    public class GameHost
    {
        public delegate void StartedHandler();
        public delegate void StoppedHandler();

        /// <summary>
        /// Called when server is started
        /// </summary>
        public event StartedHandler OnStarted;

        /// <summary>
        /// Called when server is stopped (for many reasons)
        /// </summary>
        public event StoppedHandler OnStopped;

        private String address;
        private int port;
        private Thread thread;
        private Boolean running = false;
        private GameService gameService;

        /// <summary>
        /// Creates and pre-configures the host, without starting it yet.
        /// </summary>
        /// <param name="address">address to bind TCP socket to</param>
        /// <param name="port">port to serve TCP connections at</param>
        public GameHost(String address, int port)
        {
            this.address = address;
            this.port = port;
            thread = new Thread(Run);
            thread.IsBackground = true;
            gameService = new GameService();
        }

        /// <summary>
        /// Starts the host, any event invocation can be expected after this moment
        /// </summary>
        public void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// Stops the host and waits until the host closes and finishes
        /// </summary>
        public void Stop()
        {
            running = false;
            thread.Join();
        }

        /// <summary>
        /// Method that runs the host until it is stopped by stop method
        /// </summary>
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
