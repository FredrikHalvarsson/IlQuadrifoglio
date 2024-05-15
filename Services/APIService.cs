using IlQuadrifoglio.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IlQuadrifoglio.Services
{
    public class APIService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public APIService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = clientFactory.CreateClient("API Client");
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddJwtTokenToRequest()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        //Handle Login
        public async Task<bool> LoginAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("api/account/login", new LoginModel { Username = username, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                var token = tokenResponse?.Token;

                if (!string.IsNullOrEmpty(token))
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddMinutes(30)
                    };
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("jwtToken", token, cookieOptions);

                    return true;
                }
            }
            return false;
        }

        //// Example method to call a protected API endpoint
        //public async Task<string> GetProtectedResourceAsync()
        //{
        //    AddJwtTokenToRequest();
        //    var response = await _client.GetAsync("api/protected/resource");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return await response.Content.ReadAsStringAsync();
        //    }
        //    return null;
        //}


        //Handle Register
        public async Task<bool> RegisterAsync(string username, string password, string email)
        {
            var response = await _client.PostAsJsonAsync("api/account/register", new { Username = username, Password = password, Email = email });
            return response.IsSuccessStatusCode;
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

        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
