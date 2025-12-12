# Etapa 1: Compilar el proyecto Blazor WebAssembly
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos del proyecto
COPY ["EstiloLibreFront.csproj", "./"]
RUN dotnet restore "EstiloLibreFront.csproj"

# Copiar todo el código fuente
COPY . .

# Publicar la aplicación Blazor WebAssembly
RUN dotnet publish "EstiloLibreFront.csproj" -c Release -o /app/publish

# Etapa 2: Nginx para servir archivos estáticos
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copiar archivos publicados de Blazor
COPY --from=build /app/publish/wwwroot .

# Copiar y reemplazar appsettings
COPY --from=build /app/publish/wwwroot/appsettings.Docker.json ./appsettings.json

# Copiar scripts
COPY docker-entrypoint.sh /docker-entrypoint.sh
COPY replace-env.sh /replace-env.sh
RUN chmod +x /docker-entrypoint.sh /replace-env.sh

# Railway proporciona PORT como variable de entorno
ENV PORT=80

ENTRYPOINT ["/docker-entrypoint.sh"]