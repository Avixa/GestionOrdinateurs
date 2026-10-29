FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ./TP5.API/TP5.API.csproj ./TP5.API/
COPY ./TP5.Shared/TP5.Shared.csproj ./TP5.Shared/
RUN dotnet restore ./TP5.API
RUN dotnet restore ./TP5.Shared

COPY ./TP5.API/ ./TP5.API/
COPY ./TP5.Shared/ ./TP5.Shared/
RUN dotnet publish ./TP5.API -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5108

ENTRYPOINT ["dotnet", "TP5.API.dll"]