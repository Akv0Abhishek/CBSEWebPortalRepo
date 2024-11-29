FROM node:20 as build
WORKDIR /app
COPY package*.json .
RUN npm install
COPY . .
RUN npm run build

# Use an official Nginx image to serve the React app
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 8080
CMD ["nginx", "-g", "daemon off;"]
