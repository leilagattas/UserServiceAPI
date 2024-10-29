using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
MySqlServerVersion serverVersion =  new (new Version(8, 0, 23));


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

if (dbUser != null)
{
    connectionString = connectionString.Replace("${DB_USER}", dbUser);
}

if (dbPassword != null)
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

// Learn more about configuring Swagger / OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
