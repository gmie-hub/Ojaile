using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ojaile.Abstraction;
using Ojaile_WebApi.Data;
using Ojaile_WebApi.Model;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddScoped<IPropertyItemService, IPropertyItemService>();
//builder.Services.AddTransient<IPropertyItemService, IPropertyItemService>();
//builder.Services.AddSingleton<IPropertyItemService, IPropertyItemService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddCors();
//builder.Services.addb

var connectionString = builder.Configuration.GetConnectionString("DefaultConnections");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>
    (options => options.SignIn.RequireConfirmedAccount = true && options.Password.RequireDigit).
    AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => options.TokenValidationParameters
    = new TokenValidationParameters
    {
        //ValidateIssuerSigningKey = true,
        //ValidateIssuer = true,
        //ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
    });
   
    //builder.Configuration.Bind("JwtConfig", options));
    //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    //builder.Configuration.Bind(nameof(CookieAuthenticationDefaults.AuthenticationScheme)));

//Google Authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.TokenEndpoint;
//})
//.AddGoogle(options =>
//{
//    options.ClientId = "";
//    options.ClientSecret = "";
//});

//Twitter Authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = TwitterDefaults.AuthenticationScheme;
//    //options.DefaultChallengeScheme = GoogleDefaults.TokenEndpoint;
//}).AddTwitter(options =>
//{
//    options.ConsumerKey = "";
//    options.ConsumerSecret = "";
//});

//Facebook Authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = FacebookDefaults.TokenEndpoint;
//}).AddFacebook(options =>
//{
//    options.AppId = "";
//    options.AppSecret = "";
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Response from first run middleware");
//}
//    );
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Response from second run middleware");
//}
//    );
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
    app.UseHttpLogging();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//endpoints.MapControllers());

app.MapControllers();

app.Run();
