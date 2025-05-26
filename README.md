# MyFinance

## Descripción
MyFinance es una aplicación web para control financiero personal. Permite registrar ingresos y gastos, generar resúmenes mensuales y visualizar un dashboard dinámico.

## Características
- Registro de transacciones: fecha, categoría (ingreso/gasto), monto.
- Resumen mensual automático de ingresos, egresos y saldo restante.
- Dashboard interactivo con gráficos y alertas de desviaciones presupuestarias.
- Arquitectura limpia (hexagonal) con capas separadas: Domain, Application, Infrastructure, API, Client.
- Frontend: Blazor WebAssembly (.NET 7).
- Backend: ASP.NET Core Web API (.NET 7).
- Persistencia: EF Core + SQL Server.

## Estructura del proyecto
```
MyFinance/
├─ src/
│  ├─ MyFinance.Domain/           Entidades, objetos de valor, interfaces.
│  ├─ MyFinance.Application/      Casos de uso, servicios, DTOs.
│  ├─ MyFinance.Infrastructure/   DbContext, repositorios EF Core.
│  ├─ MyFinance.API/              API REST, controladores.
│  └─ MyFinance.Client/           Blazor WASM, UI.
└─ MyFinance.sln                  Solución de Visual Studio.
```

## Requisitos
- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- SQL Server (LocalDB, SQL Express o instancia remota).
- (Opcional) Herramienta CLI de EF Core:  
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- Paquetes EF Core en **MyFinance.Infrastructure**:
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Design
  - Microsoft.EntityFrameworkCore.Tools

## Configuración y puesta en marcha

1. **Clonar el repositorio**  
   ```bash
   git clone <repo-url>
   cd MyFinance
   ```

2. **Configurar la cadena de conexión** en `MyFinance.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=TU_SERVIDOR;Database=MyFinanceDb;User Id=USUARIO;Password=CLAVE;TrustServerCertificate=True;"
     }
   }
   ```

3. **Ejecutar migraciones y crear la base de datos**  
   ```bash
   dotnet ef migrations add InitialCreate      --project src/MyFinance.Infrastructure/MyFinance.Infrastructure.csproj      --startup-project src/MyFinance.API/MyFinance.API.csproj

   dotnet ef database update      --project src/MyFinance.Infrastructure/MyFinance.Infrastructure.csproj      --startup-project src/MyFinance.API/MyFinance.API.csproj
   ```

4. **Ejecutar la API**  
   ```bash
   cd src/MyFinance.API
   dotnet run
   ```
   - Swagger UI disponible en `https://localhost:5001/swagger`.

5. **Ejecutar el cliente Blazor**  
   ```bash
   cd ../MyFinance.Client
   dotnet run
   ```
   - Cliente accesible en `https://localhost:5001/transactions` y `/dashboard`.

## Uso de la API

- **TransactionsController**  
  - `GET /api/transactions`  
  - `POST /api/transactions`  
  - `GET /api/transactions/summary/{year}/{month}`  

- **BudgetController**  
  - `GET /api/budget`  
  - `GET /api/budget/{year}/{month}`  
  - `POST /api/budget`  
  - `DELETE /api/budget/{id}`  

## Contribuciones
¡Bienvenidas! Por favor abre un *pull request* o un *issue* para discutir cambios significativos.

## Licencia
MIT License.
