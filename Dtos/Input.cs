using Newtonsoft.Json;

namespace Quran_Sunnah_BackendAI.Dtos
{
    public class Input
    {
        [JsonProperty("question")]
        public string Question { get; set; }
    }
}
