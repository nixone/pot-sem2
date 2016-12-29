using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{

    [DataContract(Name ="Figure")]
    public enum Figure
    {
        [EnumMember]
        MAN,
        [EnumMember]
        KING,
        [EnumMember]
        NONE
    }

    [DataContract(Name = "Player")]
    public enum Player
    {
        [EnumMember]
        WHITE,
        [EnumMember]
        BLACK,
        [EnumMember]
        NONE
    }

    [DataContract]
    public class Field
    {
        [DataMember]
        public Player Player = Player.NONE;

        [DataMember]
        public Figure Figure = Figure.NONE;
    }

    [DataContract]
    public class GameState
    {
        [DataMember]
        public List<Field> _board = new List<Field>();

        public GameState()
        {
            for (int i=0; i<8; i++)
            {
                for (int j=0; j<8; j++)
                {
                    _board.Add(new pot_sem2.Field());
                }
            }
        }

        public Field this[int x, int y]
        {
            get
            {
                return _board[8 * x + y];
            }
            set
            {
                _board[8 * x + y] = value;
            }
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGameService
    {
        private Random random = new Random();

        public GameState GetCurrentState()
        {
            GameState result = new GameState();
            int x = random.Next(8);
            int y = random.Next(8);
            result[x,y].Player = Player.WHITE;
            result[x,y].Figure = Figure.MAN;
            return result;
        }
    }
}
