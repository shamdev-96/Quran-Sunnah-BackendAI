using Quran_Sunnah_BackendAI.Dtos;
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
        private readonly IEmailServices _emailServices;
        private readonly Supabase.Client _supabaseClient;
        private bool _isInitialized;

        public SupabaseDatabaseServices(IConfiguration configuration, IEmailServices emailServices)
        {
            var url = configuration["SUPABASE_URL"];
            var key = configuration["SUPABASE_APIKEY"];
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
            };
            _emailServices = emailServices;
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
                var emailContent = new EmailExceptionContent
                {
                    StackTrace = ex.StackTrace,
                    ExceptionDateTime = DateTime.Now,
                };

                //_emailServices.SendExceptionEmail(emailContent);
                _isInitialized = false;
            }
        }

        public async Task InsertQuestionData(QuestionData modelData)
        {
            try
            {
                await _supabaseClient.From<Models.QuestionData>().Insert(modelData);
            }
            catch (Exception ex)
            {
                var emailContent = new EmailExceptionContent
                {
                    StackTrace = ex.StackTrace,
                    ExceptionDateTime = DateTime.Now,
                };

                //_emailServices.SendExceptionEmail(emailContent);
            }
    
        }
    }
}
