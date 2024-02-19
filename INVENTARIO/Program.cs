using INVENTARIO;
using INVENTARIO.Interfaces;
using INVENTARIO.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var cadena = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddDbContext<SampleContext>(
    options =>
    {
        options.UseSqlServer(cadena);
    });

// Register services with appropriate lifetimes
builder.Services.AddSingleton<cifrado>(); // Singleton
builder.Services.AddScoped<ITokenService, TokenService>(); // Register ITokenService

/*List<string> CorsOriginAllowed = builder.Configuration.GetSection("AllowedOrigins").Get<List<string>>();
string[] origins = CorsOriginAllowed != null ? CorsOriginAllowed.ToArray() : new string[] { "*" };

builder.Services.AddCors(options =>
{

    options.AddPolicy("CorsPolicy",
        builder => builder
        .WithOrigins(origins)
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        );
});*/

var app = builder.Build();

// Use your custom CORS middleware
app.UseCorsMiddleware();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<cifrado>();
builder.Services.AddTransient<TokenService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "INVENTARIO v1"));
}


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
