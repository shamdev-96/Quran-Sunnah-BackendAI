using Quran_Sunnah_BackendAI.Dtos;

namespace Quran_Sunnah_BackendAI.Interfaces
{
    public interface IQuranSunnahBackendAPI
    {
        bool Active { get; }
        bool IsInitialized { get; }
        Task<AskPayloadResponse> SendRequestAsync(AskPayloadRequest payloadRequest);
    }
}
