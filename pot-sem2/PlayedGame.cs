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
    [DataContract]
    public class PlayedGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }

        [DataMember]
        public String WhitePlayerName { get; set; }

        [DataMember]
        public String BlackPlayerName { get; set; }

        [DataMember]
        public String WinnerName { get; set; }

        [DataMember]
        [NotMapped]
        public List<GameState> Replay { get; set; }

        [DataMember]
        public String ReplayAsXml { get; set; }

        public PlayedGame()
        {

        }

        public PlayedGame(String whitePlayerName, String blackPlayerName, String winnerName, List<GameState> replay)
        {
            WhitePlayerName = whitePlayerName;
            BlackPlayerName = blackPlayerName;
            WinnerName = winnerName;
            Replay = replay;

            ReplayAsXml = Serialize(replay);
        }

        public List<GameState> GetReplay()
        {
            if ((Replay == null || Replay.Count == 0) && ReplayAsXml != null)
            {
                return Deserialize(ReplayAsXml, typeof(List<GameState>)) as List<GameState>;
            }
            return Replay;
        }

        public static string Serialize(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static object Deserialize(string xml, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(toType);
                return deserializer.ReadObject(stream);
            }
        }
    }
}
