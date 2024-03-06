using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
            // retrieve ai key from configuration
            var openAiKey = _configuration["OPENAI_API_KEY"];

            if (openAiKey == null)
            {
                return NotFound("Key not found");
            }

            if (string.IsNullOrEmpty(request.Question))
            {
                return BadRequest("No question is detected in the request");
            }


            var openai = new ChatGpt(openAiKey , new ChatGptOptions { MaxTokens = 4096L});
            if (string.IsNullOrEmpty(request.Language))
            {
                return BadRequest("No language selection is detected in the request");
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
                    return NotFound("unable to retrieve answer from the source");
                }

                return Ok(new AskPayloadResponse() { Answer = response });
            }

            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
    }
}
