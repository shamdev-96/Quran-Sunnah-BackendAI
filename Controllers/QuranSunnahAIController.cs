using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quran_Sunnah_BackendAI.Dtos;

namespace Quran_Sunnah_BackendAI.Controllers
{
    [EnableCors("MyCorsPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class QuranSunnahAIController : ControllerBase
    {
        private readonly ILogger<QuranSunnahAIController> _logger;
        private IConfiguration _configuration;
        public QuranSunnahAIController(ILogger<QuranSunnahAIController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;          
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
        public async Task<IActionResult> AskAI([FromBody] AskPayloadRequest request)
        {

            var listKeys = _configuration.GetSection("OPENAI_API_KEYS").Get<List<string>>();

            var resultData = new ResultData();
            int retryCount = 0;
            int retryLimit = listKeys!.Count - 1;
            //int retryLimitLoop = 0;

            foreach (var openAiKey in listKeys!)
            {

                if (openAiKey == null)
                {
                    resultData.StatusCode = System.Net.HttpStatusCode.NotFound;
                    resultData.Result = "Key not found";
                    break;
                }

                if (string.IsNullOrEmpty(request.Question))
                {
                    resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    resultData.Result = "No question is detected in the request";
                    break;
                }

                var openai = new ChatGpt(openAiKey, new ChatGptOptions { MaxTokens = 1000L });

                if (string.IsNullOrEmpty(request.Language))
                {
                    resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    resultData.Result = "No language selection is detected in the request";
                    break;
                }

                string completedQuestion;

                switch (request.Language)
                {
                    case "BM":
                        completedQuestion = $"Berikan jawapan untuk soalan ini berpandukan Al-Quran dan Hadis dan sertakan dengan pautan yang berkaitan dengan jawapan: {request.Question}";
                        break;
                    case "EN":
                        completedQuestion = $"Find any answer for this question based on Quran and Hadith and give any related links about the answer: {request.Question}";
                        break;
                    default:
                        return BadRequest("The language selection is not valid");
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
                    resultData.Result = new AskPayloadResponse() { Answer = response };
                    break;
                }

                catch (HttpRequestException ex)
                {
                    if(retryCount < retryLimit &&  ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        retryCount++;
                        Console.WriteLine($"Response from API Key: {openAiKey} FAILED . RETRY FOR {retryCount} time(s)...");
                        continue;
                    }
                    //else if (retryLimitLoop < 3)
                    //{
                    //    retryCount = 0;
                    //    retryLimitLoop++;
                    //    goto Retry;
                    //}

                    resultData.StatusCode = ex.StatusCode;
                    resultData.Result = ex.Message;
                    break;
                    //else
                    //{
                       
                    //}  
                }
            }

            switch (resultData.StatusCode)
            {
                case System.Net.HttpStatusCode.OK: 
                    return Ok (resultData.Result);
                case System.Net.HttpStatusCode.BadRequest:
                    return BadRequest(resultData.Result);
                case System.Net.HttpStatusCode.NotFound:
                    return NotFound(resultData.Result);
                default:
                    return Problem(resultData.Result!.ToString());
            }
        }
    }
}
