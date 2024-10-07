using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Quran_Sunnah_BackendAI.Models
{
    [Table("question-answer")]
    public class QuestionData : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("question")]
        public string? Question { get; set; }

        [Column("answer")]
        public string? Answer { get; set; }

        [Column("isSucessResponse")]
        public bool IsSuccessResponse { get; set; }

        [Column("requestDateTime")]
        public DateTime RequestDateTime { get; set; }

        [Column("responseTimeSeconds")]
        public double ResponseTimeSeconds { get; set; }

    }
}
