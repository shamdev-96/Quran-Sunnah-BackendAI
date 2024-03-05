using Quran_Sunnah_BackendAI.Dtos;
using System.Text.RegularExpressions;

namespace Quran_Sunnah_BackendAI.Helpers
{
    public static class MessageFormatterHelper
    {
        internal static string FormatAnswerByAppendingTag(string answerResponse)
        {
            string pattern = @"(http|https|ftp|file)://[\w-]+(\.[\w-]+)+([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

            // Create a Regex object
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Replace URLs with <a> tags
            return regex.Replace(answerResponse, "<a href=\"$&\">$&</a>");
        }
        internal static string FormatAnswerByExtractingUrl(string answerResponse)
        {
            string pattern = @"(http|https|ftp|file)://[\w-]+(\.[\w-]+)+([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            MatchCollection matches = regex.Matches(answerResponse);

            return regex.Replace(answerResponse, "");

            //var answerResponse = new AskPayloadResponse
            //{
            //    Answer = clearResponse
            //};

            //foreach (Match match in matches)
            //{
            //    answerResponse.SourceLinks.Add(match.Value);
            //}
        }
    }
}
