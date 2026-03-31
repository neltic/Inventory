# ETAPA 1: Construcción
FROM node:22-alpine AS build-step
WORKDIR /app

# Copiamos archivos de dependencias
COPY package.json package-lock.json ./
RUN npm install

# Copiamos el resto del código y compilamos
COPY . .
RUN npm run build -- --configuration=production

# ETAPA 2: Servidor Web (Nginx)
FROM nginx:alpine
# Borramos el contenido por defecto de nginx
RUN rm -rf /usr/share/nginx/html/*

# Copiamos el build de Angular a la carpeta de Nginx
COPY --from=build-step /app/dist/app/browser /usr/share/nginx/html

# Copiamos una configuración básica de Nginx para manejar rutas de Angular
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]