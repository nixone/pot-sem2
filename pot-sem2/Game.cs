using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{

    public enum Figure
    {
        MAN, KING, NONE
    }

    public enum Player
    {
        WHITE, BLACK, NONE
    }

    public class Field
    {
        public Player Player = Player.NONE;
        public Figure Figure = Figure.NONE;
    }

    public class GameState
    {
        public Field[,] Board = new Field[8,8];
    }

    public class Game
    {
        private Random random = new Random();

        public GameState GetGameState()
        {
            GameState result = new GameState();
            int x = random.Next(8);
            int y = random.Next(8);
            result.Board[x, y].Player = Player.WHITE;
            result.Board[x, y].Figure = Figure.MAN;
            return result;
        }
    }
}
