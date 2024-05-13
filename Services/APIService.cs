namespace IlQuadrifoglio.Services
{
    public class APIService
    {
        private readonly HttpClient _client;
        public APIService(IHttpClientFactory clientfactory)
        {
            _client = clientfactory.CreateClient("API Client");
        }

    }
}
