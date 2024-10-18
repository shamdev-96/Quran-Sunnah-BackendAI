namespace Quran_Sunnah_BackendAI.Helpers
{
    public class ApiLoadBalancer
    {
        private readonly List<string> _apiKeys;
        private int _requestCount;
        private const int MaxRequestsPerKey = 10;

        public ApiLoadBalancer( List<string> apiKeys)
        {
            _apiKeys = apiKeys;
            _requestCount = 0;
        }

        public string GetApiKey()
        {
            // Determine which API key to use based on the request count
            int keyIndex = (_requestCount / MaxRequestsPerKey) % _apiKeys.Count;
            _requestCount++;
            Console.WriteLine($"Open AI request using API Key number {keyIndex}");
            return _apiKeys[keyIndex];
        }
    }
}
