using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserServiceAPI.Common.Filters;
using UserServiceAPI.Infrastructure;
using UserServiceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

#region Filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});
#endregion

#region DefaultConnection
MySqlServerVersion serverVersion = new(new Version(8, 0, 23));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dbSchema = Environment.GetEnvironmentVariable("DB_SCHEMA");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

if (!string.IsNullOrEmpty(dbSchema))
{
    connectionString = connectionString.Replace("${DB_SCHEMA}", dbSchema);
}

if (!string.IsNullOrEmpty(dbUser))
{
    connectionString = connectionString.Replace("${DB_USER}", dbUser);
}

if (!string.IsNullOrEmpty(dbPassword))
{
    connectionString = connectionString.Replace("${DB_PASSWORD}", dbPassword);
}

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

#endregion

#region DI Services && Repository 
builder.Services.AddDIServices();
builder.Services.AddRepository();

#endregion

#region AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region Authentication
var jwt_configuration = builder.Configuration.GetSection("Jwt");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

if (!string.IsNullOrEmpty(jwtKey))
{
    jwt_configuration["Key"] = jwtKey;
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

#endregion


// Learn more about configuring Swagger / OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter your Bearer token below:\n\nExample: Bearer \"{your token}\"",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
