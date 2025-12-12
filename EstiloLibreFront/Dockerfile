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

# Reemplazar appsettings.json con appsettings.Docker.json para Docker
RUN rm -f appsettings.json && mv appsettings.Docker.json appsettings.json

# Copiar configuración de Nginx
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80