using AdminPanel.Code;
using AdminPanel.Services;
using AminPanel.Services.Category;
using AminPanel.Services.ProductFeature;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Tewr.Blazor.FileReader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IDeleteFileService, DeleteFileService>();
builder.Services.AddScoped<AuthenticationStateProvider, AccountAuthentication>();
builder.Services.AddScoped<IToastMessage, ToastMessage>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ProductFeatureService>();
builder.Services.AddScoped(typeof(IRootApi<>), typeof(RootApi<>));
builder.Services.AddScoped<CategoryService>();

builder.Services.AddSweetAlert2();
builder.Services.AddFileReaderService();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapRazorPages();
app.Run();
