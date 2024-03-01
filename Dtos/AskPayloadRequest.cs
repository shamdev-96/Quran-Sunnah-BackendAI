using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Quran_Sunnah_BackendAI.Dtos
{
    public class AskPayloadRequest
    {
        public AskPayloadRequest()
        {
            Question = string.Empty;
            Language = string.Empty;
        }

        /// <summary>
        /// Question query to ask to OpenAI
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Language selection to be used to ask in OpenAI. Only supported string of BM/EN
        /// </summary>
        public string Language { get; set; }
    }
}
