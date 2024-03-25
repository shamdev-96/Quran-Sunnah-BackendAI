//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ActionConstraints;
//using Quran_Sunnah_BackendAI.Dtos;
//using System.Net;

//namespace Quran_Sunnah_BackendAI.Helpers
//{
//    internal class ResponseGeneratorHelper : ControllerBase
//    {
//        [HttpPost("ask")]
//        internal static IActionResult GenerateResponseFromResult(ResultData result)
//        {
//            switch (result.StatusCode)
//            {

//                case HttpStatusCode.OK:
//                    return Ok(result.Result);
//            }
//        }
//    }
//}
