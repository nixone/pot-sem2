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
        Boolean IsWhitePlayer(int clientIndex);

        [OperationContract]
        Boolean IsBlackPlayer(int clientIndex);
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

        public Boolean IsWhitePlayer(int clientIndex)
        {
            return clientIndex == 0 && nextClientIndex > 2;
        }

        public Boolean IsBlackPlayer(int clientIndex)
        {
            return clientIndex == 1 && nextClientIndex > 2;
        }

        public int ObtainClientIndex(String clientName)
        {
            clientNames[nextClientIndex] = clientName;
            return nextClientIndex++;
        }
    }
}
