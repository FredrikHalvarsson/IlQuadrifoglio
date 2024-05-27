using IlQuadrifoglio.Models;
using Newtonsoft.Json;

namespace IlQuadrifoglio.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = clientFactory.CreateClient("API Client");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync() //Get ApplicationUsers
        {
            try
            {
                var response = await _client.GetAsync("api/ApplicationUsers");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var applicationUsers = JsonConvert.DeserializeObject<List<ApplicationUser>>(jsonString);
                    return applicationUsers;
                }
                return new List<ApplicationUser>();
            }
            catch (Exception ex)
            {
                return new List<ApplicationUser>();
            }
        }

        public async Task<ApplicationUser> GetApplicationUserByIdAsync(string id)
        {
            var response = await _client.GetAsync($"api/ApplicationUsers/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApplicationUser>();
            }
            else
            {
                throw new InvalidOperationException($"API failed with statuscode {response.StatusCode}");
            }
        }

        public async Task<bool> UpdateApplicationUserAsync(string id, ApplicationUser applicationUser)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/ApplicationUsers/{id}", applicationUser);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteApplicationUserAsync(string id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/ApplicationUsers/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
