using Quran_Sunnah_BackendAI.Interfaces;
using Quran_Sunnah_BackendAI.Models;
using Supabase;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Postgrest.Models;
using Supabase.Realtime;
using Supabase.Storage;

namespace Quran_Sunnah_BackendAI.Services
{
    public class SupabaseDatabaseServices : ISupabaseDatabaseServices
    {
        private readonly Supabase.Client _supabaseClient;
        private bool _isInitialized;
        public SupabaseDatabaseServices(IConfiguration configuration)
        {
            var url = configuration["SUPABASE_URL"];
            var key = configuration["SUPABASE_APIKEY"];
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
            };

            _supabaseClient = new Supabase.Client(url, key, options);
        }

       public bool IsInitialized => _isInitialized;

        public async Task InitializeSupabase()
        {
            try
            {
                await _supabaseClient.InitializeAsync();
               _isInitialized = true;
            }
            catch (Exception ex)
            {
                string exMsg = ex.ToString() ;
                _isInitialized = false;
            }
        }

        public async Task<bool> InsertQuestionData(QuestionData modelData)
        {
            bool isSuccess;
            try
            {
                await _supabaseClient.From<Models.QuestionData>().Insert(modelData);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                //TODO: to handle the exception here properly
                isSuccess = false;
            }         
            return isSuccess;
        }
    }
}
