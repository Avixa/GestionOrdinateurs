FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ./AuthentificationApi/AuthentificationApi.csproj ./AuthentificationApi/
COPY ./TP5.Shared/TP5.Shared.csproj ./TP5.Shared/
RUN dotnet restore ./AuthentificationApi
RUN dotnet restore ./TP5.Shared

COPY ./AuthentificationApi/ ./AuthentificationApi/
COPY ./TP5.Shared/ ./TP5.Shared/
RUN dotnet publish ./AuthentificationApi -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5299

ENTRYPOINT ["dotnet", "AuthentificationApi.dll"]