# backend/Dockerfile

# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY AuthenticationAPI/*.csproj ./AuthenticationAPI/
RUN dotnet restore ./AuthenticationAPI/*.csproj

COPY AuthenticationAPI/. ./AuthenticationAPI/
WORKDIR /app/AuthenticationAPI
RUN dotnet publish -c Release -o /app/publish

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthAPI.dll"]