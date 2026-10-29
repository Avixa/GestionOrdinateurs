FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ./TP5.Gateway/TP5.Gateway.csproj ./TP5.Gateway/
RUN dotnet restore ./TP5.Gateway

COPY ./TP5.Gateway/ ./TP5.Gateway/
RUN dotnet publish ./TP5.Gateway -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5010

ENTRYPOINT ["dotnet", "TP5.Gateway.dll"]