FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CBSEWebPortal.csproj", "./"]
RUN dotnet restore "./CBSEWebPortal.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CBSEWebPortal.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CBSEWebPortal.dll"]
