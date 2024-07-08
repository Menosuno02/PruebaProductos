using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PruebaProductos.Server.Data;
using PruebaProductos.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Configuración del CORS
var corsPolicyName = "AllowAngularDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, builder =>
    {
        builder.WithOrigins("https://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configuración de otros servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<RepositoryProductos>();

// Configuración del DbContext
string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<ProductosContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configuración del middleware y pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(corsPolicyName); // Aplica la política CORS configurada

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
