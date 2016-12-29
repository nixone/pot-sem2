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
        public delegate void NewStateHandler(GameState state, Player player);

        public event ConnectedHandler OnConnected;
        public event DisconnectedHandler OnDisconnected;
        public event NewStateHandler OnNewState;

        private Thread thread;

        private String name;
        private String address;
        private int port;
        private Boolean running = false;
        private int clientIndex;
        private IGameService service = null;

        public GameClient(String name, String address, int port)
        {
            this.name = name;
            this.address = address;
            this.port = port;
            this.thread = new Thread(Run);
            thread.IsBackground = true;
        }

        public void Start()
        {
            if (running)
            {
                return;
            }
            thread.Start();
        }

        public void Stop()
        {
            if (!running)
            {
                return;
            }
            running = false;
            thread.Join();
        }

        public void Select(int x, int y)
        {
            if (!running)
            {
                return;
            }
            service.Select(x, y, clientIndex);
        }

        public void FinishTurn()
        {
            if (!running)
            {
                return;
            }
            service.FinishTurn(clientIndex);
        }

        public void Run()
        {
            running = true;

            try
            {
                using (var channelFactory = new ChannelFactory<IGameService>("GameService", new EndpointAddress("net.tcp://" + address + ":" + port + "/game")))
                {
                    service = channelFactory.CreateChannel();

                    clientIndex = service.ObtainClientIndex(name);

                    if (OnConnected != null)
                    {
                        OnConnected();
                    }

                    while (running)
                    {
                        GameState state = service.GetCurrentState();
                        Player player = service.GetPlayer(clientIndex);

                        if (OnNewState != null)
                        {
                            OnNewState(state, player);
                        }
                        Thread.Sleep(500);
                    }
                }
            }
            catch
            {
            }

            service = null;
            running = false;

            if (OnDisconnected != null)
            {
                OnDisconnected();
            }
        }
    }
}
