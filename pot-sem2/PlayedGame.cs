using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace pot_sem2
{
    /// <summary>
    /// Representation of a recorded game
    /// </summary>
    [DataContract]
    public class PlayedGame
    {
        /// <summary>
        /// Key for identification purposes in database
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }

        /// <summary>
        /// Name of white player in the game
        /// </summary>
        [DataMember]
        public String WhitePlayerName { get; set; }

        /// <summary>
        /// Name of black player in the game
        /// </summary>
        [DataMember]
        public String BlackPlayerName { get; set; }

        /// <summary>
        /// Name of the winner
        /// </summary>
        [DataMember]
        public String WinnerName { get; set; }

        /// <summary>
        /// Serialized replay in the database
        /// </summary>
        [DataMember]
        public String SerializedReplay { get; set; }

        /// <summary>
        /// Empty constructor for database purposes
        /// </summary>
        public PlayedGame()
        {
        }

        /// <summary>
        /// Creates a played game record
        /// </summary>
        /// <param name="whitePlayerName">white player name</param>
        /// <param name="blackPlayerName">black player name</param>
        /// <param name="winnerName">winner name</param>
        /// <param name="replay">replay</param>
        public PlayedGame(String whitePlayerName, String blackPlayerName, String winnerName, List<GameState> replay)
        {
            WhitePlayerName = whitePlayerName;
            BlackPlayerName = blackPlayerName;
            WinnerName = winnerName;

            SerializedReplay = SerializationUtil.Serialize(replay);
        }

        /// <summary>
        /// Retrieves the replay
        /// </summary>
        /// <returns>replay as list of game states</returns>
        public List<GameState> GetReplay()
        {
            return SerializationUtil.Deserialize(SerializedReplay, typeof(List<GameState>)) as List<GameState>;
        }
    }
}
