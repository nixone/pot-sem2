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
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameService : IGameService
    {
        private Game game = null;

        private int nextClientIndex = 0;

        private String[] clientNames = new String[256];

        public GameService()
        {
            game = new pot_sem2.Game();
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
    }
}
