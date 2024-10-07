using Quran_Sunnah_BackendAI.Dtos;

namespace Quran_Sunnah_BackendAI.Interfaces
{
    public interface IEmailServices
    {
        void SendExceptionEmail(EmailExceptionContent content);
    }
}
