using System.Net;

namespace Quran_Sunnah_BackendAI.Dtos
{
    public class AskPayloadResponse
    {
        public HttpStatusCode? StatusCode { get; set; }
        public string? Answer { get; set; }
    }
}
