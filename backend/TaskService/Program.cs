using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskService.Models;
using TaskService.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskService.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adicione o serviço de controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configuração do SQLite (mesma string de conexão para ambos)
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

builder.Services.AddScoped<TaskManager>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Adicione o middleware para roteamento de controladores
app.UseRouting();

// Adicione esta linha para aplicar a política CORS
app.UseCors("AllowAll");

// Adicione o middleware de autorização
app.UseAuthorization();
/*
var sampleTodos = new TodoTask[] {
    new TodoTask
    {
        Id = 1,
        Title = "Task 1",
        Description = "Description 1",
        Status = TodoTaskStatus.Aberto,
        Priority = TaskPriority.Baixa
    }
};

var todosApi = app.MapGroup("/tasks");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());
*/
// Mapeie as rotas dos controladores
app.MapControllers();

/*
app.MapPost("/tasks", async (TaskCreateDto taskDto, TaskManager taskManager) =>
{
    try 
    {
        var newTask = await taskManager.CreateTask(taskDto);
        return Results.Created($"/tasks/{newTask.Id}", newTask);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/tasks/export", async (TaskManager taskManager) =>
{
    try 
    {
        var excelBytes = await taskManager.ExportToExcel();
        return Results.File(
            excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "tarefas.xlsx"
        );
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
*/

app.Run();

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

