using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quran_Sunnah_BackendAI.Constant;
using Quran_Sunnah_BackendAI.Dtos;
using Quran_Sunnah_BackendAI.Interfaces;
using Quran_Sunnah_BackendAI.Models;
using Supabase.Storage;
using System.Diagnostics;
using System.Security.Principal;

namespace Quran_Sunnah_BackendAI.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class QuranSunnahAIController : ControllerBase
    {
        private IConfiguration _configuration;
        private static IEnumerable<IQuranSunnahBackendAPI> _providers;
        private static  IQuranSunnahBackendAPI? _activeProvider;
        private static ISupabaseDatabaseServices _supabase;
        public QuranSunnahAIController(IConfiguration configuration, IEnumerable<IQuranSunnahBackendAPI> providers , ISupabaseDatabaseServices supabase)
        {
            _configuration = configuration;

            if (_providers == null && _supabase == null && _activeProvider == null)
            {
                _providers = providers;
                _supabase = supabase;
                _activeProvider = _providers!.FirstOrDefault(p => p.Active);
            }

        }

        [HttpGet("version")]
        public string Version()
        {
            return _configuration["VERSION"]!;
        }

        /// <summary>
        /// A POST endpoint that will accept a request with params of a question and language selection
        /// and the answer will be retrieved from OpenAI integration
        /// </summary>
        /// <param name="request">Contains string of question and language selection (only BM & EN is accepted) </param>
        /// <returns></returns>
        [HttpPost("ask")]
        public async Task<AskPayloadResponse> AskAI([FromBody] AskPayloadRequest request)
        {
            var watch = new Stopwatch();
            watch.Start();

            var resultData = new AskPayloadResponse();

            //int retryLimitLoop = 0;

            if (string.IsNullOrEmpty(request.Question))
            {
                resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                resultData.Answer = "No question is detected in the request";
            }

            if (string.IsNullOrEmpty(request.Language))
            {
                resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                resultData.Answer = "No language selection is detected in the request";
            }

            resultData = await _activeProvider!.SendRequestAsync(request);

            var isSuccessResponse = resultData.StatusCode == System.Net.HttpStatusCode.OK;

            var questionData = new QuestionData
            {
                Question = request.Question,
                Answer = isSuccessResponse ? resultData.Answer : null,
                IsSuccessResponse = isSuccessResponse,
                ResponseTimeSeconds = watch.Elapsed.TotalSeconds,
                RequestDateTime = DateTime.Now
            };

            watch.Stop();

            if(!_supabase.IsInitialized)
            {
                await _supabase.InitializeSupabase();
            }

            await _supabase.InsertQuestionData(questionData);

            return resultData;

        }
    }
}
