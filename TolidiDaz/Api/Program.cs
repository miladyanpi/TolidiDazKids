using AutoMapper;
using DAL.Context;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using MappingProfile.DtoMappingConfigs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Polly;
using ServicesLibrary.Services;
using ServicesLibrary.Services.ViewCounter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.ConfigureIdentity();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddDbContext<IUnitOfWork, ApplicationDbContext>();
builder.Services.ConfigureServices();

builder.Services.ConfigureJwt(builder.Configuration);//builder.Services.AddAuthentication();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new DtoMappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IAsyncPolicy>(Policy
    .Handle<SqlException>()
    .Or<TimeoutException>()
    .CircuitBreakerAsync(
    exceptionsAllowedBeforeBreaking: 1,
    durationOfBreak: TimeSpan.FromSeconds(20)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insert Token Jwt With Bearer Into Field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
             new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id="Bearer",
                }
            },
             Array.Empty<string>()
        }


    });
});
builder.Services.AddCors();
builder.Services.AddHangfire(configuration => configuration
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
builder.Services.AddHangfireServer();
var app = builder.Build();

app.UseCors(cors => cors
.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true)
.AllowCredentials()
);
app.MapGet("/", () => "Runing Api!");
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseWebAssemblyDebugging();  ///

}
else
{
    // در Production بهتر است از این استفاده کنی
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
//app.UseBlazorFrameworkFiles();///
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Uploadfiles")),
    RequestPath = new PathString("/Uploadfiles")
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//app.UseHangfireDashboard("/HangfireDashboard");
app.UseHangfireDashboard("/HangfireDashboard", new DashboardOptions
{
    IsReadOnlyFunc = (DashboardContext context) => true,

});

//http://127.0.0.1:5218/HangfireDashboard
//app.MapFallbackToFile("index.html");///
RecurringJob.RemoveIfExists("sync-product-views");
// ثبت Recurring Job (هر 2 دقیقه یکبار)
RecurringJob.AddOrUpdate<SyncProductViewsJob>(
    "sync-product-views",
    job => job.ExecuteProductAsync(),
    "*/2 * * * *");
RecurringJob.RemoveIfExists("sync-blog-views");
// ثبت Recurring Job (هر 2 دقیقه یکبار)
RecurringJob.AddOrUpdate<SyncProductViewsJob>(
    "sync-blog-views",
    job => job.ExecuteBlogAsync(),
    "*/2 * * * *");

app.Run();
