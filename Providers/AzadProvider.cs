using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quran_Sunnah_BackendAI.Configurations;
using Quran_Sunnah_BackendAI.Dtos;
using Quran_Sunnah_BackendAI.Interfaces;

namespace Quran_Sunnah_BackendAI.Providers
{

    /// <summary>
    /// A provider class that provided by Azad to call special LLM endpoint to get the query result.
    /// </summary>
    public class AzadProvider : IQuranSunnahBackendAPI
    {
        private IConfiguration _configuration;
        public bool Active { get; private set; }

        public AzadProvider(IConfiguration configuration, IOptions<QuranSunnahProviderOptions> options)
        {
            _configuration = configuration;
            Active = options.Value.AzadProvider.Active;
        }

        public async Task<ResultData> SendRequestAsync(AskPayloadRequest payloadRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, _configuration["BASEMODEL_URL"]);

                    string apiToken = _configuration["BASEMODEL_API_TOKEN"]!;

                    string json = JsonConvert.SerializeObject(new RequestObject { Input = new Input { Question = payloadRequest.Question } }, Formatting.Indented);

                    request.Headers.Add("Authorization", apiToken);
                    var content = new StringContent(json, null, "application/json");
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
                        responseBody =  response.ReasonPhrase  ?? "Error detail is not returned by provider";
                        Console.WriteLine($"Failed to call API. Status code: {response.StatusCode}");
                    }

                    return new ResultData { StatusCode = response.StatusCode, Result =  responseBody };

                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
