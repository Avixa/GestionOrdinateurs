FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ./TP5.Client/TP5.Client.csproj ./TP5.Client/
COPY ./TP5.Shared/TP5.Shared.csproj ./TP5.Shared/
RUN dotnet restore ./TP5.Client
RUN dotnet restore ./TP5.Shared

COPY ./TP5.Client/ ./TP5.Client/
COPY ./TP5.Shared/ ./TP5.Shared/
RUN dotnet publish ./TP5.Client -c Release -o out

FROM nginx:alpine AS runtime

COPY --from=build /app/out/wwwroot /usr/share/nginx/html

COPY ./TP5.Client/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 5000