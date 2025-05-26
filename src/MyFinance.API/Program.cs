using Microsoft.EntityFrameworkCore;
using MyFinance.Infrastructure.Data;
using MyFinance.Infrastructure.Repositories;
using MyFinance.Domain.Interfaces;
using MyFinance.Application.UseCases;
using MyFinance.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Configurar DbContext con la cadena en appsettings.json
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConexion")));

// 2) Registrar repositorios (Infrastructure → Domain interfaces)
builder.Services.AddScoped<ITransactionRepository, EFCoreTransactionRepository>();
builder.Services.AddScoped<IBudgetRepository,      EFCoreBudgetRepository>();

// 3) Registrar casos de uso / servicios (Application → UseCases interfaces)
builder.Services.AddScoped<ITransactionUseCase, TransactionService>();
builder.Services.AddScoped<IBudgetUseCase,      BudgetService>();

// 4) Añadir controladores
builder.Services.AddControllers();

// (Opcional) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyFinance API v1");
        c.RoutePrefix = string.Empty;          // Swagger en la raíz (https://localhost:5001/)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();