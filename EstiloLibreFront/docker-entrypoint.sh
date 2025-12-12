#!/bin/sh
# docker-entrypoint.sh para Nginx con puerto dinámico

# Railway proporciona PORT, usar 80 como fallback
export PORT=${PORT:-80}

# Reemplazar variables de entorno en appsettings.json
/replace-env.sh

# Generar configuración completa de nginx
cat > /etc/nginx/nginx.conf << EOF
worker_processes auto;
error_log /var/log/nginx/error.log warn;
pid /var/run/nginx.pid;

events {
    worker_connections 1024;
}

http {
    include /etc/nginx/mime.types;
    default_type application/octet-stream;

    log_format main '\$remote_addr - \$remote_user [\$time_local] "\$request" '
                    '\$status \$body_bytes_sent "\$http_referer" '
                    '"\$http_user_agent" "\$http_x_forwarded_for"';

    access_log /var/log/nginx/access.log main;

    sendfile on;
    tcp_nopush on;
    keepalive_timeout 65;

    # Comprimir respuestas
    gzip on;
    gzip_vary on;
    gzip_proxied any;
    gzip_comp_level 6;
    gzip_types text/plain text/css text/xml text/javascript application/json application/javascript application/xml+rss application/rss+xml font/truetype font/opentype application/vnd.ms-fontobject image/svg+xml application/wasm;

    server {
        listen ${PORT};
        server_name _;
        
        root /usr/share/nginx/html;
        index index.html;

        # SPA - Redirigir todo a index.html para routing de Blazor
        location / {
            try_files \$uri \$uri/ /index.html;
        }

        # Archivos estáticos de Blazor - Cache largo
        location /_framework/ {
            add_header Cache-Control "public, max-age=31536000, immutable";
        }

        # Archivos CSS, JS, imágenes - Cache moderado
        location ~* \.(css|js|jpg|jpeg|png|gif|ico|svg|woff|woff2|ttf|eot)$ {
            expires 7d;
            add_header Cache-Control "public, max-age=604800";
        }

        # No cachear index.html
        location = /index.html {
            add_header Cache-Control "no-store, no-cache, must-revalidate";
            expires off;
        }

        # Seguridad
        add_header X-Frame-Options "SAMEORIGIN" always;
        add_header X-Content-Type-Options "nosniff" always;
        add_header X-XSS-Protection "1; mode=block" always;
    }
}
EOF

# Verificar configuración
echo "Verificando configuración de nginx..."
nginx -t

# Iniciar nginx
echo "Iniciando nginx en puerto ${PORT}..."
exec nginx -g 'daemon off;'