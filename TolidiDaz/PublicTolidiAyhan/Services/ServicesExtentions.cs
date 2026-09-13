using DAL.Context;
using Domain;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using PublicTolidiAyhan.Code;
using PublicTolidiAyhan.Services.ProductComment;
using PublicTolidiAyhan.Services.Customer;
using PublicTolidiAyhan.Services.Token;
using ServicesLibrary.Services.CartItemSrv;
using ServicesLibrary.Services.CartSrv;
using ServicesLibrary.Services.CustomerAddressSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderItemSrv;
using ServicesLibrary.Services.OrderPaymentTempSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.PaymentSepService;
using ServicesLibrary.Services.PricingRuleSrv;
using ServicesLibrary.Services.ProductSrv;
using ServicesLibrary.Services.SettingSrv;
using ServicesLibrary.Services.WalletSrv;
using ServicesLibrary.Services.WalletTransactionSrv;
using Utility;
using PublicTolidiAyhan.Services.BlogComment;


namespace PublicTolidiAyhan.Services
{
    public static class ServicesExtentions
    {
        public static void ConfigureServices(this IServiceCollection service)
        {

            service.AddScoped<ProtectedSessionStorage>();
            service.AddScoped<LoginStatusCheckService>();
            service.AddScoped<TokenService>();
            service.AddScoped<EncryptionMethodService>();
            service.AddScoped(typeof(IRootApi<>), typeof(RootApi<>));
            service.AddScoped<IToastMessage, ToastMessage>();
            service.AddScoped<CartPublicService>();
            service.AddScoped<CartCalculatorPriceService>();
            service.AddScoped<VerifyTransactionService>();
            service.AddScoped<AuthenticationStateProvider, AccountAuthentication>();

            service.AddScoped<FavoritUserProduct_PublicService>();

            service.AddTransient<ICustomerService, CustomerService>();

            service.AddTransient<IOrderPaymentTempService, OrderPaymentTempService>();
            service.AddTransient<ICartService, CartService>();
            service.AddTransient<ICartItemService, CartItemService>();
            service.AddTransient<IOrderService, OrderService>();
            service.AddTransient<IOrderItemService, OrderItemService>();
            service.AddTransient<ICustomerAddressService, CustomerAddressService>();
            service.AddTransient<IWalletService, WalletService>();
            service.AddTransient<IWalletTransactionService, WalletTransactionService>();
            service.AddTransient<IProductService, ProductService>();
            service.AddTransient<IPricingRuleService, PricingRuleService>();
            service.AddTransient<BlogCommentPublicService>();
            service.AddTransient<ProductCommentPublicService>();

            service.AddScoped<ISettingService, SettingService>();
            service.AddScoped<PublicSettingService>();
            service.AddScoped<AboutService>();
            service.AddScoped<TeamService>();
            service.AddScoped<CategoryService>();
            service.AddScoped<AdvertisementService>();
            service.AddScoped<AdvertisementSingleService>();
            service.AddScoped<Product_Service>();
            service.AddScoped<CustomerPublicService>();

            //service.AddTransient<IRecurringJobManager, RecurringJobManager>();
        }
        public static void ConfigureIdentity(this IServiceCollection service)
        {
            service.AddIdentityCore<Account>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
        //public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var jwtSettings = configuration.GetSection("jwtSettings");
        //    services.AddAuthentication(o =>
        //    {
        //        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    }).AddJwtBearer(o =>
        //    {
        //        o.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = jwtSettings["validIssuer"],
        //            ValidAudience = jwtSettings["validAudience"],
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"]))
        //        };
        //    });
        //}
    }
}
