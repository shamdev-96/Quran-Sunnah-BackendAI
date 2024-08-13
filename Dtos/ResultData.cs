using System.Net;

namespace Quran_Sunnah_BackendAI.Dtos
{
    public class ResultData
    {
        public HttpStatusCode? StatusCode { get; set; }
        public string? Result { get; set; }
    }
}
