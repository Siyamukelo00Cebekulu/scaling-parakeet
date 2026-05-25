using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.Api.Data;
using UserService.Api.Middleware;
using UserService.Api.Repositories;
using UserService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// configuration sections
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// DbContext
builder.Services.AddDbContext<UserServiceDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("UserServiceConnnectionString")));

// Repositories & services
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICustomerProfileService, CustomerProfileService>();

// password hasher
builder.Services.AddScoped(typeof(Microsoft.AspNetCore.Identity.IPasswordHasher<UserService.Api.Models.User>), typeof(Microsoft.AspNetCore.Identity.PasswordHasher<UserService.Api.Models.User>));

// JWT Authentication
var jwtSection = configuration.GetSection("Jwt");
var key = jwtSection.GetValue<string>("Key");
if (!string.IsNullOrEmpty(key))
{
    var keyBytes = Encoding.UTF8.GetBytes(key);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection.GetValue<string>("Issuer"),
            ValidAudience = jwtSection.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });
}

// Rate limiting configuration done in middleware

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// rate limiting middleware
app.UseMiddleware<RateLimitingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
