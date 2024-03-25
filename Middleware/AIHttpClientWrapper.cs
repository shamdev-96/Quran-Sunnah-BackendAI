using Newtonsoft.Json.Linq;
using Quran_Sunnah_BackendAI.Interfaces;
using System.Text;

namespace Quran_Sunnah_BackendAI.Middleware
{

    public class AIHttpClientWrapper : IAPIHttpClientWrapper
    {
        private IConfiguration _configuration;

        private static string _BASE_URL = "https://pms.chasm.net/api/prompts/execute/1932";

        public AIHttpClientWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SendAsync(string jsonData)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, _configuration["BASEMODEL_URL"]);

                    string apiToken = _configuration["BASEMODEL_API_TOKEN"]!;

                    request.Headers.Add("Authorization", apiToken);
                    var content = new StringContent(jsonData, null, "application/json");
                    request.Content = content;

                    // Sending a POST request to the API
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Checking if the response is successful (status code in the range 200-299)
                    string responseBody = string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        // Reading the response content as a string
                        JObject jsonObject = JObject.Parse(result);

                        // Extract value of 'content' key
                        responseBody = (string)jsonObject["content"]!;

                        Console.WriteLine("Response from API:");
                        Console.WriteLine(responseBody);
                        
                    }
                    else
                    {
                        Console.WriteLine($"Failed to call API. Status code: {response.StatusCode}");
                    }

                    return responseBody;

                }
                catch (HttpRequestException ex)
                {
                    return ex.Message;
                }

            }
        }
    }
}
