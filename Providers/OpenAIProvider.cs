using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Microsoft.Extensions.Options;
using Quran_Sunnah_BackendAI.Configurations;
using Quran_Sunnah_BackendAI.Dtos;
using Quran_Sunnah_BackendAI.Helpers;
using Quran_Sunnah_BackendAI.Interfaces;
using System.Diagnostics.Eventing.Reader;

namespace Quran_Sunnah_BackendAI.Providers
{
    public class OpenAIProvider : IQuranSunnahBackendAPI
    {
        private IConfiguration _configuration;
        public bool Active { get; private set; }
        public bool IsInitialized { get; private set; }

        private readonly ApiLoadBalancer _loadBalancer;
        private List<string> _apiKeys;

        public OpenAIProvider(IConfiguration configuration, IOptions<QuranSunnahProviderOptions> options)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                _configuration = configuration;
                Active = options.Value.OpenAIProvider.Active;

                _apiKeys = _configuration.GetSection("OPENAI_API_KEYS").Get<List<string>>()!;

                _loadBalancer = new ApiLoadBalancer(_apiKeys);

                if (Active)
                {
                    Console.WriteLine("Our Backend AI Activated using OpenAIProvider");
                }
            }
        }
        public async Task<AskPayloadResponse> SendRequestAsync(AskPayloadRequest payloadRequest)
        {
            int retryCount = 0;
            int retryLimit = _apiKeys!.Count - 1;
            int retryLimitLoop = 0;

            var resultData = new AskPayloadResponse();

        Retry:
            foreach (var openAiKey in _apiKeys!)
            {

                if (openAiKey == null)
                {
                    resultData.StatusCode = System.Net.HttpStatusCode.NotFound;
                    resultData.Answer = "Key not found";
                    break;
                }

                var apiKey = _loadBalancer.GetApiKey();

                var openai = new ChatGpt(openAiKey, new ChatGptOptions { MaxTokens = 1500L });

                if (string.IsNullOrEmpty(payloadRequest.Language))
                {
                    resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    resultData.Answer = "No language selection is detected in the request";
                    break;
                }

                string completedQuestion;

                switch (payloadRequest.Language)
                {
                    case "BM":
                        completedQuestion = $"Berikan jawapan untuk soalan ini di dalam Bahasa Malaysia berpandukan Al-Quran dan Hadis dan sertakan dengan pautan yang berkaitan dengan jawapan: {payloadRequest.Question}";
                        break;
                    case "EN":
                        completedQuestion = $"Find any answer for this question based on Quran and Hadith and give any related links about the answer: {payloadRequest.Question}";
                        break;
                    default:
                        return new AskPayloadResponse { StatusCode = System.Net.HttpStatusCode.BadRequest , Answer = "The language selection is not valid" };
                }

                try
                {
                    var response = await openai.Ask(completedQuestion);

                    if (response == null)
                    {
                        resultData.StatusCode = System.Net.HttpStatusCode.NotFound;
                        resultData.Answer = "unable to retrieve answer from the source";
                        break;
                    }

                    resultData.StatusCode = System.Net.HttpStatusCode.OK;
                    Console.WriteLine($"Response from OpenAI SUCCESS");
                    resultData.Answer =  response;
                    break;
                }

                catch (HttpRequestException ex)
                {
                    if (retryCount < retryLimit && ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        retryCount++;
                        Console.WriteLine($"Response from OpenAI FAILED with Exception:  {ex.Message} . RETRY FOR {retryCount} time(s)...");
                        continue;
                    }
                    else if (retryLimitLoop < 3)
                    {
                        retryCount = 0;
                        retryLimitLoop++;
                        goto Retry;
                    }
                    else
                    {
                        resultData.StatusCode = ex.StatusCode;
                        resultData.Answer = ex.Message;
                        break;
                    }
                }
            }

            return resultData;
        }
    }
}
