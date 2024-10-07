namespace Quran_Sunnah_BackendAI.Dtos
{
    public class EmailExceptionContent
    {
        public string Subject { get; set; } = "Quuran Sunnah AI Exceptrion";
        public DateTime ExceptionDateTime { get; set; }
        public string? StackTrace { get; set; }
    }
}
