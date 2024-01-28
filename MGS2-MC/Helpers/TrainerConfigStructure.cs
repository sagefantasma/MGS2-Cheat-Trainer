using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MGS2_MC
{
    internal class TrainerConfigStructure
    {
        public static string TrainerConfigFileLocation = "TrainerConfig.json";
        public readonly static TrainerConfig DefaultConfig = new TrainerConfig
        {
            AutoLaunchGame = true,
            CloseGameWithTrainer = false,
            CloseTrainerWithGame = true,
            Mgs2ExePath = "C:/Program Files (x86)/Steam/steamapps/common/MGS2/METAL GEAR SOLID2.exe"
        };

        public class TrainerConfig
        {
            [JsonPropertyName("autoLaunchGame")]
            public bool AutoLaunchGame { get; set; }
            [JsonPropertyName("closeGameWithTrainer")]
            public bool CloseGameWithTrainer { get; set; }
            [JsonPropertyName("closeTrainerWithGame")]
            public bool CloseTrainerWithGame { get; set; }
            [JsonPropertyName("mgs2ExePath")]
            public string Mgs2ExePath { get; set; }

            public TrainerConfig() { }

            public TrainerConfig(TrainerConfig config)
            {
                AutoLaunchGame = config.AutoLaunchGame;
                CloseGameWithTrainer = config.CloseGameWithTrainer;
                CloseTrainerWithGame = config.CloseTrainerWithGame;
                Mgs2ExePath = config.Mgs2ExePath;
            }
        }

        public static bool BuildTrainerConfigFile(string fileLocation, TrainerConfig baseConfig = null)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileLocation))
                {
                    if (baseConfig == null)
                    {
                        baseConfig = new TrainerConfig(DefaultConfig);
                    }
                    writer.WriteLine(JsonSerializer.Serialize(baseConfig));

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
