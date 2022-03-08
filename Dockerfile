#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["lancer-resources-backend.csproj", "."]
RUN dotnet restore "./lancer-resources-backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "lancer-resources-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lancer-resources-backend.csproj" -c Release -o /app/publish

FROM base AS final

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT="production"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lancer-resources-backend.dll"]