using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    /// <summary>
    /// Kind of figure on the board
    /// </summary>
    [DataContract(Name ="Figure")]
    [Serializable]
    public enum Figure
    {
        [EnumMember]
        MAN,
        [EnumMember]
        KING,
        [EnumMember]
        NONE
    }

    /// <summary>
    /// Kind of player on the board
    /// </summary>
    [DataContract(Name = "Player")]
    [Serializable]
    public enum Player
    {
        [EnumMember]
        WHITE,
        [EnumMember]
        BLACK,
        [EnumMember]
        NONE
    }

    /// <summary>
    /// Representation of one field on the board
    /// </summary>
    [DataContract]
    [Serializable]
    public class Field
    {
        /// <summary>
        /// Player occupying this field
        /// </summary>
        [DataMember]
        public Player Player = Player.NONE;

        /// <summary>
        /// Figure occupying this field
        /// </summary>
        [DataMember]
        public Figure Figure = Figure.NONE;

        /// <summary>
        /// If this field is selected by somebody
        /// </summary>
        [DataMember]
        public Boolean Selected = false;

        /// <summary>
        /// Empty required constructor
        /// </summary>
        public Field()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="from"></param>
        public Field(Field from)
        {
            Player = from.Player;
            Figure = from.Figure;
            Selected = from.Selected;
        }
    }

    /// <summary>
    /// Representation of a single state of game at certain time, game as seen as replay for example is seen
    /// as list of game states.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GameState
    {
        /// <summary>
        /// List of 64 fields of the board.
        /// </summary>
        [DataMember]
        public List<Field> _board = new List<Field>();
        
        /// <summary>
        /// Player whos turn it is.
        /// </summary>
        [DataMember]
        public Player PlayerOnTurn = Player.NONE;
    
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="from"></param>
        public GameState(GameState from)
        {
            PlayerOnTurn = from.PlayerOnTurn;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _board.Add(new pot_sem2.Field(from[i, j]));
                }
            }
        }

        /// <summary>
        /// Initial game state constructor. This constructor also ensures the initial figure and player state
        /// for usual game of draughts.
        /// </summary>
        public GameState()
        {
            for (int i=0; i<8; i++)
            {
                for (int j=0; j<8; j++)
                {
                    _board.Add(new pot_sem2.Field());
                }
            }

            for (int i=0; i<4; i++)
            {
                this[i * 2, 0].Figure = Figure.MAN;
                this[i * 2, 0].Player = Player.WHITE;
                
                this[i * 2 + 1, 1].Figure = Figure.MAN;
                this[i * 2 + 1, 1].Player = Player.WHITE;

                this[i * 2, 6].Figure = Figure.MAN;
                this[i * 2, 6].Player = Player.BLACK;
                
                this[i * 2 + 1, 7].Figure = Figure.MAN;
                this[i * 2 + 1, 7].Player = Player.BLACK;
            }

            PlayerOnTurn = Player.WHITE;
        }

        /// <summary>
        /// Operator allowing access to specific board field.
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y">row</param>
        /// <returns>requested field</returns>
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

        /// <summary>
        /// Figures out the winner of the current game state
        /// </summary>
        /// <returns>Player.NONE if nobody won, or the winner player</returns>
        public Player GetWinner()
        {
            Boolean foundWhite = false;
            Boolean foundBlack = false;

            for (int i=0; i<8; i++)
            {
                for (int j=0; j<8; j++)
                {
                    if (this[i, j].Player == Player.WHITE)
                    {
                        foundWhite = true;
                    }
                    if (this[i, j].Player == Player.BLACK)
                    {
                        foundBlack = true;
                    }
                }
            }

            if (foundWhite && foundBlack)
            {
                return Player.NONE;
            }
            if (foundWhite)
            {
                return Player.WHITE;
            }
            return Player.BLACK;
        }

        /// <summary>
        /// Figures out if the game state represents a finished game
        /// </summary>
        /// <returns></returns>
        public Boolean IsFinished()
        {
            return GetWinner() != Player.NONE;
        }
    }

    /// <summary>
    /// Current progress of game, is used for simulation purposes and for interacting with game states.
    /// </summary>
    public class Game
    {
        private List<GameState> replay = new List<GameState>();

        private GameState state = new GameState();

        private List<int[]> selectedOnes = new List<int[]>();
        private Figure selectedFigure = Figure.NONE;
        private Boolean didFinishSelection = false;

        /// <summary>
        /// Creates a default game
        /// </summary>
        public Game()
        {
            replay.Add(new GameState(state));
        }

        /// <summary>
        /// Gets a replay up until current point of the game.
        /// </summary>
        /// <returns>replay as list of GameStates</returns>
        public List<GameState> GetReplay()
        {
            return new List<GameState>(replay);
        }

        /// <summary>
        /// Gets a current state of the game 
        /// </summary>
        /// <returns>current state of the game</returns>
        public GameState GetCurrentState()
        {
            return new GameState(state);
        }

        /// <summary>
        /// Selecting a field [x, y] as a player. This operation will either change a current game state 
        /// or leave it unchanged. Method call is not returning validity of the selection itself.
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y">row</param>
        /// <param name="player">player making selection</param>
        public void Select(int x, int y, Player player)
        {
            if (player != state.PlayerOnTurn)
            {
                return;
            }

            // If we reselect the last, delete the whole selection
            if (selectedOnes.Count > 0)
            {
                int[] last = selectedOnes.Last();
                if (x == last[0] && y == last[1])
                {
                    // Reset selection state
                    selectedOnes.Clear();
                    selectedFigure = Figure.NONE;
                    didFinishSelection = false;
                    for (int i=0; i<8; i++)
                    {
                        for (int j=0; j<8; j++)
                        {
                            state[i, j].Selected = false;
                        }
                    }
                    return;
                }
            }

            // If we are already selecting what was selected
            if (state[x, y].Selected)
            {
                Console.WriteLine("Reselecting!");
                return;
            }

            // We are selecting a new figure
            if (selectedFigure == Figure.NONE)
            {
                // Its not our figure
                if (state[x, y].Player != player)
                {
                    Console.WriteLine("Its not our figure!");
                    return;
                }
                selectedFigure = state[x, y].Figure;
                didFinishSelection = false;
            }
            // We are continuing in selection
            else
            {
                if (didFinishSelection)
                {
                    Console.WriteLine("Hey, you finished your selection! Finish your turn!");
                    return;
                }
                // There is somebody occupying
                if (state[x, y].Figure != Figure.NONE)
                {
                    Console.WriteLine("There is somebody!");
                    return;
                }

                int[] last = selectedOnes.Last();
                int dx = x - last[0];
                int dy = y - last[1];

                // We are not moving diagonally
                if (Math.Abs(dx) != Math.Abs(dy))
                {
                    Console.WriteLine("Not going diagonally!");
                    return;
                }

                // We are going with the man
                if (selectedFigure == Figure.MAN)
                {
                    // If we move in backwards direction
                    if ((player == Player.WHITE && dy < 0) || (player == Player.BLACK && dy > 0))
                    {
                        Console.WriteLine("Wrong way!");
                        return;
                    }

                    // We move more than 2
                    if (Math.Abs(dy) > 2)
                    {
                        Console.WriteLine("Man cannot move more than 2!");
                        return;
                    }

                    // If we are jumping over nobody
                    if (Math.Abs(dy) == 2 && state[last[0]+dx/2, last[1]+dy/2].Player == Player.NONE)
                    {
                        Console.WriteLine("Man cannot jump over nobody");
                        return;
                    }

                    if (Math.Abs(dy) == 1)
                    {
                        didFinishSelection = true;
                    }
                }
                // We are going with the king
                else
                {
                    int j = last[1];
                    Boolean jumpingOverSomebody = false;
                    for (int i = last[0]; i != x; i += dx / Math.Abs(dx))
                    {
                        if (i == last[0])
                        {
                            continue;
                        }
                          
                        if (state[i, j].Player == player)
                        {
                            Console.WriteLine("King cannot jump over himself!");
                            return;
                        }
                        if (state[i, j].Player != Player.NONE)
                        {
                            jumpingOverSomebody = true;
                        }
                        j += dy / Math.Abs(dy);
                    }
                    if (!jumpingOverSomebody)
                    {
                        didFinishSelection = true;
                    }
                }
            }

            selectedOnes.Add(new int[] { x, y });
            state[x, y].Selected = true;
        }

        /// <summary>
        /// Method for finishing a turn with preious selection. Also, as selection, this
        /// method either changes a current state or not, not throwing exceptions or return values.
        /// </summary>
        /// <param name="player">player we are finishing a turn for</param>
        public void FinishTurn(Player player)
        {
            if (player != state.PlayerOnTurn)
            {
                Console.WriteLine("Not our turn!");
                return;
            }

            if (selectedOnes.Count == 0)
            {
                Console.WriteLine("Cannot finish empty turn!");
                return;
            }

            int x = selectedOnes[0][0];
            int y = selectedOnes[0][1];

            for (int i=1; i<selectedOnes.Count; i++)
            {
                int tx = selectedOnes[i][0];
                int ty = selectedOnes[i][1];
                int dx = tx - x;
                int dy = ty - y;
                int yy = y;
                for (int xx=x; xx != tx; xx += dx/Math.Abs(dx))
                {
                    state[xx, yy].Player = Player.NONE;
                    state[xx, yy].Figure = Figure.NONE;
                    yy += dy / Math.Abs(dy);
                }
                state[tx, ty].Player = player;
                state[tx, ty].Figure = selectedFigure;

                x = tx;
                y = ty;
            }

            // If we reach other end, change to king
            if ((player == Player.WHITE && y == 7) || (player == Player.BLACK && y == 0))
            {
                state[x, y].Figure = Figure.KING;
            }

            // Reset selection state
            selectedOnes.Clear();
            selectedFigure = Figure.NONE;
            didFinishSelection = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    state[i, j].Selected = false;
                }
            }

            // Move turn to next one
            if (player == Player.WHITE)
            {
                state.PlayerOnTurn = Player.BLACK;
            }
            else
            {
                state.PlayerOnTurn = Player.WHITE;
            }

            replay.Add(new GameState(state));
        }
    }
}
