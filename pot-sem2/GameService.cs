using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract]
        GameState GetCurrentState();

        [OperationContract]
        int ObtainClientIndex(String name);

        [OperationContract]
        Player GetPlayer(int clientIndex);

        [OperationContract]
        void Select(int x, int y, int clientIndex);

        [OperationContract]
        void FinishTurn(int clientIndex);

        [OperationContract]
        String GetPlayerName(Player player);

        [OperationContract]
        void StartNewGame();

        [OperationContract]
        void Record();

        [OperationContract]
        List<PlayedGame> GetPlayedGames();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameService : IGameService
    {
        private Game game = null;

        private int nextClientIndex = 0;

        private String[] clientNames = new String[256];

        private List<PlayedGame> playedGames = new List<PlayedGame>();

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
