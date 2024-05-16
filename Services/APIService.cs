﻿using IlQuadrifoglio.Models;
using Newtonsoft.Json;
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
        public async Task<bool> LoginAsync(string username, string password, bool rememberMe)
        {
            var response = await _client.PostAsJsonAsync("api/account/login", new LoginModel { Username = username, Password = password, RememberMe = rememberMe });
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
        public async Task<bool> RegisterAsync(string username, string password, string confirmPassword, string email)
        {
            var payload = new
            {
                email = email,
                password = password,
                confirmPassword = confirmPassword,
                username = username
            };

            var response = await _client.PostAsJsonAsync("api/account/register", payload);
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

        ////////////////////////////////////////////
        ///////        Ingredients         ////////
        //////////////////////////////////////////

        public async Task<List<Ingredient>> GetIngredientsAsync() //Get ingredients
        {
            try
            {
                var response = await _client.GetAsync("api/ingredients");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonString);
                    return ingredients;
                }
                return new List<Ingredient>();
            }
            catch (Exception ex)
            {
                return new List<Ingredient>();
            }
        }

        public async Task<bool> CreateIngredientAsync(Ingredient ingredient)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/ingredients", ingredient);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Ingredient> GetIngredientByIdAsync(int id)
        {
            var response = await _client.GetAsync($"api/ingredients/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Ingredient>();
            }
            else
            {
                throw new InvalidOperationException($"API failed with statuscode {response.StatusCode}");
            }
        }

        public async Task<bool> UpdateIngredientAsync(int id, Ingredient ingredient)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/ingredients/{id}", ingredient);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteIngredientAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/ingredients/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //OrderProducts

        public async Task<List<OrderProduct>> GetOrderProductsAsync() //Get OrderProducts
        {
            try
            {
                var response = await _client.GetAsync("api/orderProduct");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var orderProducts = JsonConvert.DeserializeObject<List<OrderProduct>>(jsonString);
                    return orderProducts;
                }
                return new List<OrderProduct>();
            }
            catch (Exception ex)
            {
                return new List<OrderProduct>();
            }
        }

        public async Task<bool> CreateOrderProductAsync(OrderProduct orderProduct)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/orderProduct", orderProduct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<OrderProduct> GetOrderProductByIdAsync(int id)
        {
            var response = await _client.GetAsync($"api/orderProducts/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderProduct>();
            }
            else
            {
                throw new InvalidOperationException($"API failed with statuscode {response.StatusCode}");
            }
        }

        public async Task<bool> UpdateOrderProductAsync(int id, OrderProduct orderProduct)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/orderProducts/{id}", orderProduct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteOrderProductAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/orderProducts/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
