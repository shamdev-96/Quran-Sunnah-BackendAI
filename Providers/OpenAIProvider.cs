﻿using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Microsoft.Extensions.Options;
using Quran_Sunnah_BackendAI.Configurations;
using Quran_Sunnah_BackendAI.Dtos;
using Quran_Sunnah_BackendAI.Interfaces;

namespace Quran_Sunnah_BackendAI.Providers
{
    public class OpenAIProvider : IQuranSunnahBackendAPI
    {
        private IConfiguration _configuration;
        public bool Active { get; private set; }

        public OpenAIProvider(IConfiguration configuration, IOptions<QuranSunnahProviderOptions> options)
        {
            _configuration = configuration;
            Active = options.Value.OpenAIProvider.Active;
        }
        public async Task<ResultData> SendRequestAsync(AskPayloadRequest payloadRequest)
        {
            var listKeys = _configuration.GetSection("OPENAI_API_KEYS").Get<List<string>>();
            int retryCount = 0;
            int retryLimit = listKeys!.Count - 1;
            int retryLimitLoop = 0;

            var resultData = new ResultData();

        Retry:
            foreach (var openAiKey in listKeys!)
            {

                if (openAiKey == null)
                {
                    resultData.StatusCode = System.Net.HttpStatusCode.NotFound;
                    resultData.Result = "Key not found";
                    break;
                }

                var openai = new ChatGpt(openAiKey, new ChatGptOptions { MaxTokens = 1000L });

                if (string.IsNullOrEmpty(payloadRequest.Language))
                {
                    resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    resultData.Result = "No language selection is detected in the request";
                    break;
                }

                string completedQuestion;

                switch (payloadRequest.Language)
                {
                    case "BM":
                        completedQuestion = $"Berikan jawapan untuk soalan ini berpandukan Al-Quran dan Hadis dan sertakan dengan pautan yang berkaitan dengan jawapan: {payloadRequest.Question}";
                        break;
                    case "EN":
                        completedQuestion = $"Find any answer for this question based on Quran and Hadith and give any related links about the answer: {payloadRequest.Question}";
                        break;
                    default:
                        return new ResultData { StatusCode = System.Net.HttpStatusCode.BadRequest , Result = "The language selection is not valid" };
                }

                try
                {
                    var response = await openai.Ask(completedQuestion);

                    if (response == null)
                    {
                        resultData.StatusCode = System.Net.HttpStatusCode.NotFound;
                        resultData.Result = "unable to retrieve answer from the source";
                        break;
                    }

                    resultData.StatusCode = System.Net.HttpStatusCode.OK;
                    Console.WriteLine($"Response from API Key: {openAiKey} SUCCESS. READY FOR NEXT...");
                    resultData.Result =  response;
                    break;
                }

                catch (HttpRequestException ex)
                {
                    if (retryCount < retryLimit && ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        retryCount++;
                        Console.WriteLine($"Response from API Key: {openAiKey} FAILED . RETRY FOR {retryCount} time(s)...");
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
                        resultData.Result = ex.Message;
                        break;
                    }
                }
            }

            return resultData;
        }
    }
}