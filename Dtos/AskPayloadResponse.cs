namespace Quran_Sunnah_BackendAI.Dtos
{
    public class AskPayloadResponse
    {
        public AskPayloadResponse()
        {
            SourceLinks = new();
        }

        public string? Answer { get; set; }
        public List<string> SourceLinks { get; set; }
    }
}
