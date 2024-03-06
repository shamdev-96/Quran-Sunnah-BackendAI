using System.Net;

namespace Quran_Sunnah_BackendAI.Dtos
{
    public class ResultData
    {
        public HttpStatusCode? StatusCode { get; set; }
        public object? Result { get; set; }
    }
}
