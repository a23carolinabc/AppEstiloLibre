#!/bin/sh
# docker-entrypoint.sh para Nginx con puerto dinámico

# Railway proporciona PORT, usar 80 como fallback
export PORT=${PORT:-80}

# Reemplazar variables de entorno en appsettings.json
/replace-env.sh

# Reemplazar el template de nginx con el puerto correcto
envsubst '${PORT}' < /etc/nginx/templates/default.conf.template > /etc/nginx/conf.d/default.conf

# Iniciar nginx
exec nginx -g 'daemon off;'