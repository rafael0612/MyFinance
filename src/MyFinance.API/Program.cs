using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Application.Services;
using MyFinance.Application.UseCases;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;
using MyFinance.Infrastructure.Repositories;
using MyFinance.Infrastructure.Services;
using MyFinance.Shared.Config;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) Registramos CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5278")    // URL de tu Client Blazor
            .AllowAnyHeader()                         // permitir cabeceras
            .AllowAnyMethod();                        // permitir GET, POST, DELETE, etc.
    });
});

// 2) Configurar DbContext con la cadena en appsettings.json
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Email IMAP settings desde appsettings.json
builder.Services.Configure<EmailImapSettings>(
    builder.Configuration.GetSection("EmailImapSettings"));

// 3) Registrar repositorios (Infrastructure → Domain interfaces)
builder.Services.AddScoped<ITransactionRepository, EFCoreTransactionRepository>();
builder.Services.AddScoped<IBudgetRepository, EFCoreBudgetRepository>();
builder.Services.AddScoped<IEmailNotificationParserService, EmailNotificationParserService>();
builder.Services.AddScoped<ICsvExcelImportService, CsvExcelImportService>();
builder.Services.AddScoped<IUserRepository, EFCoreUserRepository>();

// 4) Registrar casos de uso / servicios (Application → UseCases interfaces)
builder.Services.AddScoped<ITransactionUseCase, TransactionService>();
builder.Services.AddScoped<IBudgetUseCase, BudgetService>();
builder.Services.AddScoped<IBulkTransactionImportUseCase, BulkTransactionImportUseCase>();
builder.Services.AddScoped<IEmailTransactionImportUseCase, EmailTransactionImportUseCase>();
builder.Services.AddScoped<IFinancialIndicatorUseCase, FinancialIndicatorUseCase>();
builder.Services.AddScoped<IUserRegisterUseCase, UserRegisterService>();
builder.Services.AddScoped<IUserLoginUseCase, UserLoginService>();

// 5) Añadir controladores
builder.Services.AddControllers();

// (Opcional) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// 6) Configurar autenticación JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
        };
    });

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
else
{

}
// 
app.UseRouting();
// 7) Habilitamos CORS globalmente
app.UseCors("AllowClientApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    dbContext.Database.Migrate(); // Aplicar migraciones pendientes
}
app.Run();