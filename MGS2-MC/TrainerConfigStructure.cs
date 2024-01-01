using System.Text.Json.Serialization;

namespace MGS2_MC
{
    internal class TrainerConfigStructure
    {
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
        }
    }
}
