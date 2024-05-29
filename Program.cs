using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;

namespace IlQuadrifoglio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            var apiClientBaseAddress = builder.Configuration["ApiClientBaseAddress"];
            builder.Services.AddHttpClient("API Client", client =>
            {
                client.BaseAddress = new Uri(apiClientBaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddScoped<APIService>();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();


            builder.Services.AddTransient<LocationService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //app.MapRazorPages();

            app.Run();
        }
    }
}
