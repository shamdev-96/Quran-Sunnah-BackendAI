namespace Quran_Sunnah_BackendAI.Interfaces
{
    public interface IMongoDbServices
    {
       Task<bool> InsertData<T>(string collectionName, T dataToInsert);
    }
}
