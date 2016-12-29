using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pot_sem2
{
    public class GameClient
    {
        public delegate void ConnectedHandler();
        public delegate void DisconnectedHandler();
        public delegate void NewStateHandler(GameState state);

        public event ConnectedHandler OnConnected;
        public event DisconnectedHandler OnDisconnected;
        public event NewStateHandler OnNewState;

        private Thread thread;

        private String address;
        private int port;
        private Boolean running = false;

        public GameClient(String address, int port)
        {
            this.address = address;
            this.port = port;
            this.thread = new Thread(Run);
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

            Thread.Sleep(1000);

            Console.WriteLine("Client connecting");
            using (var channelFactory = new ChannelFactory<IGameService>("GameService", new EndpointAddress("net.tcp://"+address+":"+port+"/game")))
            {
                IGameService service = channelFactory.CreateChannel();

                if (OnConnected != null)
                {
                    OnConnected();
                }

                while (running)
                {
                    GameState state = service.GetCurrentState();
                    if (OnNewState != null)
                    {
                        OnNewState(state);
                    }
                    Thread.Sleep(250);
                }

                if (OnDisconnected != null)
                {
                    OnDisconnected();
                }
            }
            running = false;
        }
    }
}
