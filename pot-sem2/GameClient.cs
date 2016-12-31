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
    /// Representing a client connected to some game host
    /// </summary>
    public class GameClient
    {
        public delegate void ConnectedHandler();
        public delegate void DisconnectedHandler();
        public delegate void NewStateHandler(GameState state, Player player, String playerNameWhite, String playerNameBlack);
        public delegate void NewPlayedGamesHandler(List<PlayedGame> playedGames);

        /// <summary>
        /// Called when client is succesfully connected to host
        /// </summary>
        public event ConnectedHandler OnConnected;

        /// <summary>
        /// Called whenever client is disconnected (for many reasons)
        /// </summary>
        public event DisconnectedHandler OnDisconnected;

        /// <summary>
        /// Called when new game state is observed, somewhere between being connected and disconnected
        /// </summary>
        public event NewStateHandler OnNewState;

        /// <summary>
        /// Called when new played games list is observed, somewhere between being connected and disconnected
        /// </summary>
        public event NewPlayedGamesHandler OnNewPlayedGames;

        private Thread thread;

        private String name;
        private String address;
        private int port;
        private Boolean running = false;
        private int clientIndex;
        private IGameService service = null;

        /// <summary>
        /// Creates a client and pre-configures it, but doesn't connect it yet.
        /// </summary>
        /// <param name="name">name of player</param>
        /// <param name="address">address to connect to (ip or dns)</param>
        /// <param name="port">port to connect to</param>
        public GameClient(String name, String address, int port)
        {
            this.name = name;
            this.address = address;
            this.port = port;
            this.thread = new Thread(Run);
            thread.IsBackground = true;
        }

        /// <summary>
        /// Starts the client, after this point we can expect events any time
        /// </summary>
        public void Start()
        {
            if (running)
            {
                return;
            }
            thread.Start();
        }

        /// <summary>
        /// Stops the client and waits for the client to stop until this method returns.
        /// </summary>
        public void Stop()
        {
            if (!running)
            {
                return;
            }
            running = false;
            thread.Join();
        }

        /// <summary>
        /// Invokes selection of specific tile and waits until the server returns.
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y">row</param>
        public void Select(int x, int y)
        {
            if (!running)
            {
                return;
            }
            service.Select(x, y, clientIndex);
        }

        /// <summary>
        /// Finished the turn and waits until the server returns.
        /// </summary>
        public void FinishTurn()
        {
            if (!running)
            {
                return;
            }
            service.FinishTurn(clientIndex);
        }

        /// <summary>
        /// Triggers a server to store a currently finished game and waits until the server returns.
        /// </summary>
        public void Record()
        {
            if (!running)
            {
                return;
            }
            service.Record();
        }

        /// <summary>
        /// Main method that runs the client, periodically checks the new state of game and triggers necessary events.
        /// </summary>
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
                        String whiteName = service.GetPlayerName(Player.WHITE);
                        String blackName = service.GetPlayerName(Player.BLACK);

                        if (OnNewState != null)
                        {
                            OnNewState(state, player, whiteName, blackName);
                        }
                        if (OnNewPlayedGames != null)
                        {
                            OnNewPlayedGames(service.GetPlayedGames());
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
