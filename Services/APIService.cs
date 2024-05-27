using IlQuadrifoglio.Models;
using Newtonsoft.Json;


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

        public async Task<bool> LoginAsync(string username, string password, bool rememberMe)
        {
            var response = await _client.PostAsJsonAsync("api/account/login", new LoginModel { Username = username, Password = password, RememberMe = rememberMe });
            if (response.IsSuccessStatusCode)
            {
                var authCookie = response.Headers.GetValues("Set-Cookie");
                foreach (var cookie in authCookie)
                {
                    _httpContextAccessor.HttpContext.Response.Headers.Append("Set-Cookie", cookie);
                }
                return true;
            }
            return false;
        }

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

        public async Task<ApplicationUser> GetUserAsync(string username)
        {
            var response = await _client.GetAsync($"api/account/getuser?username={username}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<ApplicationUser>(jsonString);
                return user;
            }
            return null;
        }

        public async Task<List<string>> GetUserRolesAsync(string username)
        {
            var response = await _client.GetAsync($"api/account/getroles?username={username}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<string>>(jsonString);
                return roles;
            }
            return new List<string>();
        }

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



        ////////////////////////////////////////////
        ///////        Products         ////////
        //////////////////////////////////////////

        public async Task<List<Product>> GetProductsAsync() //Get Products
        {
            try
            {
                var response = await _client.GetAsync("api/products");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<Product>>(jsonString);
                    return products;
                }
                return new List<Product>();
            }
            catch (Exception ex)
            {
                return new List<Product>();
            }
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/products", product);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _client.GetAsync($"api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            else
            {
                throw new InvalidOperationException($"API failed with statuscode {response.StatusCode}");
            }
        }

        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/products/{id}", product);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/products/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        ////////////////////////////////////////////
        ///////      OrderProducts       ////////
        //////////////////////////////////////////

        public async Task<List<OrderProduct>> GetOrderProductsAsync() //Get OrderProducts
        {
            try
            {
                var response = await _client.GetAsync("api/orderProducts");
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
                var response = await _client.PostAsJsonAsync("api/orderProducts", orderProduct);
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


        ////////////////////////////////////////////
        ///////           Order            ////////
        //////////////////////////////////////////

        public async Task<List<Order>> GetOrderAsync() //Get order
        {
            try
            {
                var response = await _client.GetAsync("api/orders");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<Order>>(jsonString);
                    return orders;
                }
                return new List<Order>();
            }
            catch (Exception ex)
            {
                return new List<Order>();
            }
        }
        public async Task<Order> GetLatestOrderAsync(string username)
        {
            try
            {
                var response = await _client.GetAsync($"api/orders/latest/{username}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var order = JsonConvert.DeserializeObject<Order>(jsonString);
                    return order;
                }
                return new Order();
            }
            catch (Exception ex)
            {
                return new Order();
            }
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/orders", order);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var response = await _client.GetAsync($"api/orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Order>();
            }
            else
            {
                throw new InvalidOperationException($"API failed with statuscode {response.StatusCode}");
            }
        }

        public async Task<bool> UpdateOrderAsync(int id, Order order)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/orders/{id}", order);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/orders/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
