using Assessment_BE_Engineer_3_Web;
using Assessment_BE_Engineer_3_Web.Services;
using Assessment_BE_Engineer_3_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddHttpClient<IFileService, FileService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication
    (options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //options.DefaultChallengeScheme = "oidc";
    })
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Auth/Login";
                  options.AccessDeniedPath = "/Auth/AccessDenied";
                  options.SlidingExpiration = true;
              });
              //.AddOpenIdConnect("oidc", options => {
              //    options.RequireHttpsMetadata = false;
              //    options.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
              //    options.GetClaimsFromUserInfoEndpoint = true;
              //    options.ClientId = "fileapi";
              //    options.ClientSecret = "secret";
              //    options.ResponseType = "code";

              //    options.TokenValidationParameters.NameClaimType = "name";
              //    options.TokenValidationParameters.RoleClaimType = "role";
              //    options.Scope.Add("fileapi");
              //    options.SaveTokens = true;

              //    options.ClaimActions.MapJsonKey("role", "role");

              //    options.Events = new OpenIdConnectEvents
              //    {
              //        OnRemoteFailure = context =>
              //        {
              //            context.Response.Redirect("/");
              //            context.HandleResponse();
              //            return Task.FromResult(0);
              //        }
              //    };
              //});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
