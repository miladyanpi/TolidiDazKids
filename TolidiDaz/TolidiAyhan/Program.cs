using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TolidiAyhan.Attributes;
using TolidiAyhan.Code;
using TolidiAyhan.Constant;
using TolidiAyhan.Services;
using TolidiAyhan.Services.Cart;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddScoped<CartService>();
        builder.Services.AddScoped<IDeleteFileService, DeleteFileService>();
        builder.Services.AddScoped<AuthenticationStateProvider, AccountAuthentication>();
        builder.Services.AddScoped<IToastMessage, ToastMessage>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<TokenServiceServer>();
        //builder.Services.AddScoped<NavigationManager>();
        // Token refresh handler
        builder.Services.AddTransient<TokenRefreshHandler>();

        // ApiClient با BaseAddress
        builder.Services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri(ApiLink.fullUrl);
        })
        .AddHttpMessageHandler<TokenRefreshHandler>();

        // تزریق ApiClient به صورت مستقیم
        builder.Services.AddScoped(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return factory.CreateClient("ApiClient");
        });

        builder.Services.AddScoped(typeof(RootApi<>));
        builder.Services.AddSweetAlert2();
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddScoped<CartCalculatorPriceService>();
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<HandleUnauthenticatedFilter>();
        });
        builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenied";
            });
        var app = builder.Build();
        app.MapGet("/Logout", (HttpContext ctx) =>
        {
            ctx.Response.Cookies.Delete(AuthenticationConstant.AccessToken);
            ctx.Response.Cookies.Delete(AuthenticationConstant.RefreshToken);

            return Results.Redirect("/Login");
        });

        // Middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();   // ← اصلاح مهم

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}