using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    /// <summary>
    /// Abstract description of communication between client and host.
    /// </summary>
    [ServiceContract]
    public interface IGameService
    {
        /// <summary>
        /// Retrieves the current state of the game
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GameState GetCurrentState();

        /// <summary>
        /// Allocates new index for a client, it is expected that every client calls this endpoint just once
        /// </summary>
        /// <param name="name">player name</param>
        /// <returns>identification index</returns>
        [OperationContract]
        int ObtainClientIndex(String name);

        /// <summary>
        /// Retrieves the player information about a specific client
        /// </summary>
        /// <param name="clientIndex">index of client</param>
        /// <returns>player information, WHITE / BLACK / NONE</returns>
        [OperationContract]
        Player GetPlayer(int clientIndex);

        /// <summary>
        /// Sends a selection command to the game
        /// </summary>
        /// <param name="x">board column</param>
        /// <param name="y">board row</param>
        /// <param name="clientIndex">client index</param>
        [OperationContract]
        void Select(int x, int y, int clientIndex);

        /// <summary>
        /// Sends a finish-turn command to the game
        /// </summary>
        /// <param name="clientIndex">client index</param>
        [OperationContract]
        void FinishTurn(int clientIndex);

        /// <summary>
        /// Retrieves a player name for a specific player
        /// </summary>
        /// <param name="player">player</param>
        /// <returns>player name</returns>
        [OperationContract]
        String GetPlayerName(Player player);

        /// <summary>
        /// Sends a command to start a new game after the one was finished
        /// </summary>
        [OperationContract]
        void StartNewGame();

        /// <summary>
        /// Sends a command to record the current state of the game to played games
        /// </summary>
        [OperationContract]
        void Record();

        /// <summary>
        /// Retrieves all the played games from database
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PlayedGame> GetPlayedGames();
    }

    /// <summary>
    /// Specific implementation of communication on host side
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameService : IGameService
    {
        private Game game = null;

        private int nextClientIndex = 0;

        private String[] clientNames = new String[256];

        private List<PlayedGame> playedGames = new List<PlayedGame>();

        /// <summary>
        /// Creates a basic service with default game and refreshes the played games from database
        /// </summary>
        public GameService()
        {
            game = new pot_sem2.Game();
            RefreshPlayedGames();
        }
        
        private void RefreshPlayedGames()
        {
            playedGames.Clear();
            using (var db = new PlayedGameContext())
            {
                foreach (PlayedGame played in db.PlayedGames)
                {
                    playedGames.Add(played);
                }
            }
        }

        public GameState GetCurrentState()
        {
            return game.GetCurrentState();
        }

        public Player GetPlayer(int clientIndex)
        {
            if (clientIndex == 0)
            {
                return Player.WHITE;
            }
            if (clientIndex == 1)
            {
                return Player.BLACK;
            }
            return Player.NONE;
        }

        public int ObtainClientIndex(String clientName)
        {
            clientNames[nextClientIndex] = clientName;
            return nextClientIndex++;
        }

        public void Select(int x, int y, int clientIndex)
        {
            if (game == null)
            {
                return;
            }
            Player player = GetPlayer(clientIndex);
            if (player == Player.NONE)
            {
                return;
            }
            game.Select(x, y, player);
        }

        public void FinishTurn(int clientIndex)
        {
            if (game == null)
            {
                return;
            }
            Player player = GetPlayer(clientIndex);
            if (player == Player.NONE)
            {
                return;
            }
            game.FinishTurn(player);
        }

        public String GetPlayerName(Player player)
        {
            if (player == Player.NONE)
            {
                return "None";
            }
            return clientNames[player == Player.WHITE ? 0 : 1];
        }

        public void StartNewGame()
        {
            game = new Game();
        }

        public void Record()
        {
            if (game == null || !game.GetCurrentState().IsFinished())
            {
                return;
            }
            using(var db = new PlayedGameContext())
            {
                db.PlayedGames.Add(new PlayedGame(GetPlayerName(Player.WHITE), GetPlayerName(Player.BLACK), GetPlayerName(game.GetCurrentState().GetWinner()), game.GetReplay()));
                db.SaveChanges();
            }
            RefreshPlayedGames();
        }

        public List<PlayedGame> GetPlayedGames()
        {
            return playedGames;
        }
    }
}
