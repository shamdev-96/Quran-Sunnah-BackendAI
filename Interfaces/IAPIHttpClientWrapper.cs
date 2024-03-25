namespace Quran_Sunnah_BackendAI.Interfaces
{
    public interface IAPIHttpClientWrapper
    {
        Task<string> SendAsync(string jsonData);
    }
}
