using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using Quran_Sunnah_BackendAI.Interfaces;
using SharpCompress.Common;
using System.Collections;
using System.Xml;

namespace Quran_Sunnah_BackendAI.Services
{
    public class MongoDbServices : IMongoDbServices
    {
        private readonly IConfiguration _configuration;
        public MongoDbServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> InsertData<T>(string collectionName, T dataToInsert)
        {
            bool isSuccess = true;
            try
            {
                //var settings = MongoClientSettings.FromConnectionString(_configuration["MONGO_CONNECTIONSTRING"]);
                //settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                //var client = new MongoClient(settings);
                var client = new MongoClient(_configuration["MONGO_CONNECTIONSTRING"]);
                var database = client.GetDatabase("quransunnahdb");
                var _collection = database.GetCollection<T>(collectionName);
                await _collection.InsertOneAsync(dataToInsert, new InsertOneOptions { BypassDocumentValidation = true });
            }
            catch (Exception)
            {
               isSuccess = false;
            }

            return isSuccess;

        }
    }
}
