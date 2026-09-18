using DAL.Context;
using Domain;
using Dto.Services.ReminderEventSrv;
using Dto.Services.SmsLogSrv;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ServicesLibrary.Services.AboutSrv;
using ServicesLibrary.Services.AdvertisementSingleSrv;
using ServicesLibrary.Services.AdvertisementSrv;
using ServicesLibrary.Services.AuthenticationManagerSrv;
using ServicesLibrary.Services.BlogCommentSrv;
using ServicesLibrary.Services.BlogSrv;
using ServicesLibrary.Services.CartItemSrv;
using ServicesLibrary.Services.CartSrv;
using ServicesLibrary.Services.CategorySrv;
using ServicesLibrary.Services.CitySrv;
using ServicesLibrary.Services.ContactUsSrv;
using ServicesLibrary.Services.CustomerAddressSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.DepartmentSrv;
using ServicesLibrary.Services.FaqSrv;
using ServicesLibrary.Services.FavoritUserProductSrv;
using ServicesLibrary.Services.GroupBlogSrv;
using ServicesLibrary.Services.GroupQuestionSrv;
using ServicesLibrary.Services.OrderItemSrv;
using ServicesLibrary.Services.OrderPaymentTempSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.PersonelSrv;
using ServicesLibrary.Services.PositionSrv;
using ServicesLibrary.Services.PricingRuleSrv;
using ServicesLibrary.Services.Product_CountAction_CostTypeSrv;
using ServicesLibrary.Services.ProductCommentSrv;
using ServicesLibrary.Services.ProductFeatureSrv;
using ServicesLibrary.Services.ProductFeatureValueSrv;
using ServicesLibrary.Services.ProductSrv;
using ServicesLibrary.Services.ProductVariantSrv;
using ServicesLibrary.Services.ProductVariantValueSrv;
using ServicesLibrary.Services.ProvinceSrv;
using ServicesLibrary.Services.QuestionSrv;
using ServicesLibrary.Services.RawProductSrv;
using ServicesLibrary.Services.RawProductStore_ProductSrv;
using ServicesLibrary.Services.RawProductStoreSrv;
using ServicesLibrary.Services.RefreshTokenEntitySrv;
using ServicesLibrary.Services.RegisterCostRawProductStoreSrv;
using ServicesLibrary.Services.SendProductMethodSrv;
using ServicesLibrary.Services.SettingSrv;
using ServicesLibrary.Services.SliderSrv;
using ServicesLibrary.Services.SmsOtpCodeSrv;
using ServicesLibrary.Services.StorageSrv;
using ServicesLibrary.Services.StorySrv;
using ServicesLibrary.Services.TeamSrv;
using ServicesLibrary.Services.TicketSrv;
using ServicesLibrary.Services.TraitSrv;
using ServicesLibrary.Services.TraitValueSrv;
using ServicesLibrary.Services.ViewCounter;
using ServicesLibrary.Services.WalletSrv;
using ServicesLibrary.Services.WalletTransactionSrv;


namespace ServicesLibrary.Services
{
    public static class ServicesExtentions
    {
        public static void ConfigureServices(this IServiceCollection service)
        {
            service.AddTransient<IAuthenticationManager, AuthenticationManager>();
            service.AddIdentity<Account, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
            //service
            //   .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //   .AddCookie();

            service.AddTransient<ICustomerService, CustomerService>();
            service.AddTransient<ICategoryService, CategoryService>();
            service.AddTransient<IProductService, ProductService>();
            service.AddTransient<IStorageService, StorageService>();
            service.AddTransient<ISettingService, SettingService>();
            service.AddTransient<ISmsLogService, SmsLogService>();
            service.AddTransient<IReminderEventService, ReminderEventService>();
            service.AddTransient<ISmsOtpCodeService, SmsOtpCodeService>();
            service.AddTransient<ICartService, CartService>();
            service.AddTransient<ICartItemService, CartItemService>();
            service.AddTransient<IOrderService, OrderService>();
            service.AddTransient<IOrderItemService, OrderItemService>();
            service.AddTransient<IStoryService, StoryService>();
            service.AddTransient<ISliderService, SliderService>();
            service.AddTransient<IAdvertisementService, AdvertisementService>();
            service.AddTransient<IAdvertisementSingleService, AdvertisementSingleService>();
            service.AddTransient<IGroupBlogService, GroupBlogService>();
            service.AddTransient<IBlogService, BlogService>();
            service.AddTransient<ICustomerAddressService, CustomerAddressService>();
            service.AddTransient<ICityService, CityService>();
            service.AddTransient<IProvinceService, ProvinceService>();
            service.AddTransient<IWalletService, WalletService>();
            service.AddTransient<IWalletTransactionService, WalletTransactionService>();
            service.AddTransient<ISendProductMethodService, SendProductMethodService>();
            service.AddTransient<IOrderPaymentTempService, OrderPaymentTempService>();
            service.AddTransient<IRefreshTokenEntityService, RefreshTokenEntityService>();
            service.AddTransient<IProductFeatureService, ProductFeatureService>();
            service.AddTransient<IProductFeatureValueService, ProductFeatureValueService>();
            service.AddTransient<IPricingRuleService, PricingRuleService>();
            service.AddTransient<IPositionService, PositionService>();
            service.AddTransient<IProduct_CountAction_CostTypeService, Product_CountAction_CostTypeService>();
            service.AddTransient<IRawProductService, RawProductService>();
            service.AddTransient<IRawProductStoreService, RawProductStoreService>();
            service.AddTransient<IRegisterCostRawProductStoreService, RegisterCostRawProductStoreService>();
            service.AddTransient<IRawProductStore_ProductService, RawProductStore_ProductService>();
            service.AddTransient<IPersonelService, PersonelService>();
            service.AddTransient<IFavoritUserProductService, FavoritUserProductService>();
            service.AddTransient<IPositionService, PositionService>();
            service.AddTransient<IContactUsService, ContactUsService>();
            service.AddTransient<IAboutService, AboutService>();
            service.AddTransient<ITeamService, TeamService>();
            service.AddTransient<IFaqService, FaqService>();
            service.AddTransient<IRecurringJobManager, RecurringJobManager>();
            service.AddTransient<IGroupQuestionService, GroupQuestionService>(); 
            service.AddTransient<IQuestionService, QuestionService>();
            service.AddTransient<IProductCommentService, ProductCommentService>();
            service.AddTransient<IDepartmentService, DepartmentService>();
            service.AddTransient<ITicketService, TicketService>();
            service.AddTransient<IBlogCommentService, BlogCommentService>();
            service.AddTransient<ITraitService, TraitService>();
            service.AddTransient<ITraitValueService, TraitValueService>();
            service.AddTransient<IProductVariantService, ProductVariantService>();
            service.AddTransient<IProductVariantValueService, ProductVariantValueService>();
            service.AddSingleton<IViewCounterService, InMemoryViewCounterService>();
            service.AddScoped<SyncProductViewsJob>();

            service.AddHangfire(configuration => configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()

             .UseSqlServerStorage(ConnectionString.Value, new SqlServerStorageOptions
             {
                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                 QueuePollInterval = TimeSpan.Zero,
                 UseRecommendedIsolationLevel = true,
                 DisableGlobalLocks = true
             }));

            // Add the processing server as IHostedService
            service.AddHangfireServer();
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
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("jwtSettings");
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer",o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssure").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(jwtSettings.GetSection("secretKey").Value))
                };
            });
        }
    }
}
