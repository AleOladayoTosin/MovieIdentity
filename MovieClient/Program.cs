﻿using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MovieClient.ApiServices;
using MovieClient.Data;
using MovieClient.HttpHandlers;
using IdentityModel.Client;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMovieApiService, MovieApiService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
               {
                   options.Authority = "https://localhost:5001";

                   options.ClientId = "movie_mvc_client";
                   options.ClientSecret = "secret";
                   options.ResponseType = "code id_token";

                   //options.Scope.Add("openid");
                   //options.Scope.Add("profile");
                   options.Scope.Add("address");
                   options.Scope.Add("email");
                   options.Scope.Add("roles");

                   //options.ClaimActions.DeleteClaim("sid");
                   //options.ClaimActions.DeleteClaim("idp");
                   //options.ClaimActions.DeleteClaim("s_hash");
                   //options.ClaimActions.DeleteClaim("auth_time");
                   options.ClaimActions.MapUniqueJsonKey("role", "role");

                   options.Scope.Add("movieAPI");

                   options.SaveTokens = true;
                   options.GetClaimsFromUserInfoEndpoint = true;

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       NameClaimType = JwtClaimTypes.GivenName,
                       RoleClaimType = JwtClaimTypes.Role
                   };
               });

builder.Services.AddHttpContextAccessor();

// 1 create an HttpClient used for accessing the Movies.API
builder.Services.AddTransient<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient("MovieAPIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7262"); // API GATEWAY URL
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

// 2 create an HttpClient used for accessing the IDP
builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

//builder.Services.AddSingleton(new ClientCredentialsTokenRequest
//{
//    Address = "https://localhost:5001/connect/token",
//    ClientId = "movieClient",
//    ClientSecret = "secret",
//    Scope = "movieAPI"
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
