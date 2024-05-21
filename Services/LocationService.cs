namespace IlQuadrifoglio.Services
{
    public class LocationService
    {
        private readonly HttpClient _httpClient;

        public LocationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API Client");
        }

        public async Task<string> GetGeolocationAsync(string address)
        {
            var response = await _httpClient.GetStringAsync($"api/location/geocode?address={address}");
            return response;
        }

        public async Task<string> GetRouteAsync(string origin, string destination)
        {
            var response = await _httpClient.GetStringAsync($"api/location/route?origin={origin}&destination={destination}");
            return response;
        }
    }
}
