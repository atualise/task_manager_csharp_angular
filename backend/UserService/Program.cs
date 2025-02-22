using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Data;
using System.Text.Json.Serialization;
using UserService.Models;
using UserService.Data;
using UserService.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Adicione o serviço de controladores
builder.Services.AddControllers();

// Configuração do SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
*/

// Remova ou comente esta linha, pois TaskManager não pertence ao UserService
// builder.Services.AddScoped<TaskManager>();

// Adicione a configuração do Swagger
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Certifique-se que o banco de dados seja criado
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // Isso criará o banco de dados e as tabelas
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao criar o banco de dados.");
    }
}

// Adicione o middleware para roteamento de controladores
app.UseRouting();

// Adicione esta linha para aplicar a política CORS
app.UseCors("AllowAll");

// Adicione o middleware de autorização
//app.UseAuthorization();

// Configure o Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
    c.RoutePrefix = string.Empty; // Para acessar o Swagger UI na raiz
});

app.MapControllers();


app.Run();

[JsonSerializable(typeof(User[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

