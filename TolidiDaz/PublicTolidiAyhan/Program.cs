using AutoMapper;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using DAL.Context;
using MappingProfile.DtoMappingConfigs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using PublicTolidiAyhan.Components;
using PublicTolidiAyhan.Services;
public class SitemapUrl
{
    public string Loc { get; set; } = string.Empty;
    public DateTime? LastMod { get; set; }
    public string ChangeFreq { get; set; } = "weekly"; // always, hourly, daily, weekly, monthly, yearly, never
    public double Priority { get; set; } = 0.5;
}

internal class Program
{
    private static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddRazorComponents()
           .AddInteractiveServerComponents();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        builder.Services.ConfigureIdentity();
        builder.Services.AddDbContext<IUnitOfWork, ApplicationDbContext>();

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new DtoMappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.ConfigureServices();

        builder.Services.AddAuthentication();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSweetAlert2();
        builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
        builder.Services.AddAuthorization();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();


        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllers();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        app.MapGet("/sitemap.xml", async (HttpContext context) =>
        {
            context.Response.ContentType = "application/xml";

            var urls = new List<SitemapUrl>
    {
        new() { Loc = "https://tolidiayhan.ir/", LastMod = DateTime.UtcNow, Priority = 1.0 },
        new() { Loc = "https://tolidiayhan.ir/Shop", LastMod = DateTime.UtcNow.AddDays(-7), Priority = 0.8 },
        new() { Loc = "https://tolidiayhan.ir/Shop", LastMod = DateTime.UtcNow.AddDays(-30), Priority = 0.6 },
        // اگر داری صفحات دینامیک (مثل از دیتابیس):
        // new() { Loc = $"https://yourdomain.com/Product/{post.Slug}", LastMod = post.UpdatedAt, ... }
    };

            var xml = $@"
                    <?xml version=""1.0"" encoding=""UTF-8""?>
                    <urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
                    {string.Join("\n", urls.Select(u => $@"
                        <url>
                            <loc>{u.Loc}</loc>
                            {(u.LastMod.HasValue ? $"<lastmod>{u.LastMod.Value:yyyy-MM-dd}</lastmod>" : "")}
                            <changefreq>{u.ChangeFreq}</changefreq>
                            <priority>{u.Priority:F1}</priority>
                        </url>"))}
                    </urlset>";

            await context.Response.WriteAsync(xml);
        });

        app.Run();
    }
}
public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        // اگر unauthorized باشه، exception ننداز، فقط next() رو کال کن
        return next(context);
    }
}