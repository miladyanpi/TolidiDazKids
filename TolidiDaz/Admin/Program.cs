using Admin.Code;
using Admin.Components;
using Admin.Services;
using Admin.Services.About;
using Admin.Services.Blog;
using Admin.Services.BlogComment;
using Admin.Services.Category;
using Admin.Services.GroupBlog;
using Admin.Services.Personel;
using Admin.Services.Position;
using Admin.Services.PricingRule;
using Admin.Services.Product;
using Admin.Services.Product_CountAction_CostType;
using Admin.Services.ProductFeature;
using Admin.Services.ProductFeatureValue;


//using Admin.Services.ProductFeature;
using Admin.Services.RawProductStore_Product;
using Admin.Services.Search;
using Admin.Services.Slider;
using Admin.Services.Team;
using Admin.Services.Token;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthentication();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductFeatureService>();
builder.Services.AddScoped<EncryptionMethodService>();
builder.Services.AddScoped<PricingRuleService>();
builder.Services.AddScoped<PositionService>();
builder.Services.AddScoped<RawProductStore_ProductService>();
builder.Services.AddScoped<PersonelService>();
builder.Services.AddScoped<Product_CountAction_CostTypeService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<AboutService>();
builder.Services.AddScoped<GroupBlogService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<SliderService>();
builder.Services.AddScoped<BlogCommentService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductFeatureValueService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<DisableService>();

builder.Services.AddScoped(typeof(IRootApi<>), typeof(RootApi<>));
builder.Services.AddScoped<IToastMessage, ToastMessage>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSweetAlert2();
builder.Services.AddScoped<AuthenticationStateProvider, AccountAuthentication>();

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();


builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        // «ê— unauthorized »«‘Â° exception ‰‰œ«“° ›ﬁÿ next() —Ê ò«· ò‰
        return next(context);
    }
}