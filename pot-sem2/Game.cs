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

        [DataMember]
        public Boolean Selected = false;
    }

    [DataContract]
    public class GameState
    {
        [DataMember]
        public List<Field> _board = new List<Field>();

        [DataMember]
        public Player PlayerOnTurn = Player.NONE;

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

    
    public class Game
    {
        private Random random = new Random();

        private GameState state = new GameState();

        public GameState GetCurrentState()
        {
            state = new GameState();
            int x = random.Next(8);
            int y = random.Next(8);
            state[x, y].Player = Player.WHITE;
            state[x, y].Figure = Figure.MAN;
            state[x, y].Selected = true;
            state.PlayerOnTurn = random.Next(2) == 0 ? Player.WHITE : Player.BLACK;
            return state;
        }

        public void Select(int x, int y, Player player)
        {
            // TODO
            Console.WriteLine("TODO Selecting in game!");
            state[x, y].Selected = true;
        }

        public void FinishTurn(Player player)
        {
            // TODO
            Console.WriteLine("TODO Finishing turn in game!");
        }
    }
}
