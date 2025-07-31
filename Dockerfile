# Dockerfile para ProjectAPI - .NET 8
# Multi-stage build para optimizar el tamaño de la imagen final

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto para restaurar dependencias
COPY *.sln .
COPY API/API.csproj ./API/
COPY Business/Business.csproj ./Business/
COPY DataAccess/DataAccess.csproj ./DataAccess/
COPY Domain/Domain.csproj ./Domain/
COPY TestApi/TestApi.csproj ./TestApi/

# Restaurar dependencias
RUN dotnet restore

# Copiar todo el código fuente
COPY . .

# Compilar y publicar la aplicación
WORKDIR /app/API
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Crear usuario no-root para seguridad
RUN addgroup --system --gid 1001 dotnetgroup \
    && adduser --system --uid 1001 --ingroup dotnetgroup dotnetuser

# Crear directorio para logs y darle permisos
RUN mkdir -p /app/logs && chown -R dotnetuser:dotnetgroup /app/logs

# Copiar archivos publicados desde el stage de build
COPY --from=build /app/publish .

# Cambiar propietario de los archivos de la aplicación
RUN chown -R dotnetuser:dotnetgroup /app

# Cambiar al usuario no-root
USER dotnetuser

# Exponer el puerto
EXPOSE 8080

# Variables de entorno
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Punto de entrada
ENTRYPOINT ["dotnet", "API.dll"]