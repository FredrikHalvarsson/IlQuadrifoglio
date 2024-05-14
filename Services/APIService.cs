using IlQuadrifoglio.Models;
using Newtonsoft.Json;

namespace IlQuadrifoglio.Services
{
    public class APIService
    {
        private readonly HttpClient _client;
        public APIService(IHttpClientFactory clientfactory)
        {
            _client = clientfactory.CreateClient("API Client");
        }
        //Get all users
        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            try
            {
                var response = await _client.GetAsync("api/applicationusers");
                if (!response.IsSuccessStatusCode)
                {
                    return new List<ApplicationUser>();
                }
                var jsonstring = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<ApplicationUser>>(jsonstring);
                return users;
            }
            catch (Exception ex)
            {
                return new List<ApplicationUser>();
            }
        }

        //Create a user
        //...
    }
}
