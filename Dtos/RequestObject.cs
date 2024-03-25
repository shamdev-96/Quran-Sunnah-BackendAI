using Newtonsoft.Json;

namespace Quran_Sunnah_BackendAI.Dtos
{
    public class RequestObject
    {
        [JsonProperty("input")]
        public Input Input { get; set; }
    }
}
