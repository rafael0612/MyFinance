# Etapa 1: SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar todo
COPY . .

# 1. Restaurar
RUN dotnet restore src/MyFinance.API/MyFinance.API.csproj

# 2. Publicar cliente Blazor WASM
RUN dotnet publish src/Myfinance.Client/Myfinance.Client.csproj \
    -c Release -o /app/client

# 3. Copiar artefactos del cliente a wwwroot de la API
RUN mkdir -p src/MyFinance.API/wwwroot \
 && cp -r /app/client/wwwroot/* src/MyFinance.API/wwwroot/

# 4. Publicar la API (incluye ahora el cliente en wwwroot)
RUN dotnet publish src/MyFinance.API/MyFinance.API.csproj \
    -c Release -o /app/publish

# Etapa 2: Runtime para correr la API (+ servir el cliente)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyFinance.API.dll"]
