using Quran_Sunnah_BackendAI.Models;

namespace Quran_Sunnah_BackendAI.Interfaces
{
    public interface ISupabaseDatabaseServices
    {
        bool IsInitialized { get; }
        Task InitializeSupabase();
        Task InsertQuestionData(QuestionData modelData);
    }
}
