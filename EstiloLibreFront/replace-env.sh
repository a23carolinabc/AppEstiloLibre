#!/bin/sh
# replace-env.sh - Reemplazar variables en appsettings.json

# Obtener URL del backend desde variable de entorno
BACKEND_URL=${BACKEND_URL:-"https://backend-production-30bb.up.railway.app"}

# Reemplazar en appsettings.json
sed -i "s|\${BACKEND_URL}|${BACKEND_URL}|g" /usr/share/nginx/html/appsettings.json

echo "Variables de entorno configuradas:"
echo "BACKEND_URL: ${BACKEND_URL}"