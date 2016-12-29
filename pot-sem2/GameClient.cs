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

        public void Run()
        {
            running = true;

            try
            {
                using (var channelFactory = new ChannelFactory<IGameService>("GameService", new EndpointAddress("net.tcp://" + address + ":" + port + "/game")))
                {
                    IGameService service = channelFactory.CreateChannel();

                    int clientIndex = service.ObtainClientIndex(name);

                    if (OnConnected != null)
                    {
                        OnConnected();
                    }

                    while (running)
                    {
                        GameState state = service.GetCurrentState();
                        Player player = Player.NONE;
                        if (service.IsWhitePlayer(clientIndex))
                        {
                            player = Player.WHITE;
                        }
                        else if (service.IsBlackPlayer(clientIndex))
                        {
                            player = Player.BLACK;
                        }

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

            if (OnDisconnected != null)
            {
                OnDisconnected();
            }
            running = false;
        }
    }
}
