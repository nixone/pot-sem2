using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    [DataContract]
    public class PlayedGame
    {
        [DataMember]
        public String WhitePlayerName { get; set; }

        [DataMember]
        public String BlackPlayerName { get; set; }

        [DataMember]
        public String WinnerName { get; set; }

        [DataMember]
        public List<GameState> Replay { get; set; }

        public PlayedGame()
        {

        }

        public PlayedGame(String whitePlayerName, String blackPlayerName, String winnerName, List<GameState> replay)
        {
            WhitePlayerName = whitePlayerName;
            BlackPlayerName = blackPlayerName;
            WinnerName = winnerName;
            Replay = replay;
        }
    }
}
