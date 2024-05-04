using MongoDB.Bson.Serialization.Attributes;

namespace Quran_Sunnah_BackendAI.Dtos
{
    public class QuestionDoc
    {
        [BsonElement("question")]
        public required string Question { get; set; }

        [BsonElement("answer")]
        public string? Answer { get; set; }

        [BsonElement("request_date")]
        public DateTime RequestDate { get; set; }

        [BsonElement("response_time")]
        public long ResponseTime { get; set; }

        [BsonElement("isResponseSuccess")]
        public bool IsReponseSuccess { get; set; }
    }
}
