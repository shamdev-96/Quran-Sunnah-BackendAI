using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quran_Sunnah_BackendAI.Constant;
using Quran_Sunnah_BackendAI.Dtos;
using Quran_Sunnah_BackendAI.Interfaces;
using System.Diagnostics;
using System.Security.Principal;

namespace Quran_Sunnah_BackendAI.Controllers
{
    [EnableCors("MyCorsPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class QuranSunnahAIController : ControllerBase
    {
        private readonly ILogger<QuranSunnahAIController> _logger;
        private IConfiguration _configuration;
        private readonly IEnumerable<IQuranSunnahBackendAPI> _providers;
        public QuranSunnahAIController(ILogger<QuranSunnahAIController> logger, IConfiguration configuration, IEnumerable<IQuranSunnahBackendAPI> providers)
        {
            _logger = logger;
            _configuration = configuration;
            _providers = providers;
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
            var activeProvider = _providers.FirstOrDefault(p => p.Active);

            var resultData = new ResultData();

            //int retryLimitLoop = 0;

            if (string.IsNullOrEmpty(request.Question))
            {
                resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                resultData.Result = "No question is detected in the request";
            }


            if (string.IsNullOrEmpty(request.Language))
            {
                resultData.StatusCode = System.Net.HttpStatusCode.BadRequest;
                resultData.Result = "No language selection is detected in the request";
            }


            if(!request.Language.Equals("BM") || !!request.Language.Equals("EN"))
                return BadRequest("The language selection is not valid");

            resultData = await activeProvider!.SendRequestAsync(request);

            return new ContentResult { StatusCode = (int)resultData.StatusCode!.Value, Content = resultData.Result};

            //store result in mongoDB
            //var questionDoc = new QuestionDoc
            //{
            //    Question = request.Question,
            //    IsReponseSuccess = resultData.StatusCode == System.Net.HttpStatusCode.OK ? true : false,
            //    Answer = response,
            //    RequestDate = DateTime.Now,
            //    ResponseTime = watch.ElapsedMilliseconds
            //};

            //bool isSuccess =  await _mongoDbServices.InsertData<QuestionDoc>(MongoDbConstants.QuestionCollectionName, questionDoc);

        }
    }
}
