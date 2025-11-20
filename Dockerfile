# ==============================
# Stage 1 — Build and Publish
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ==============================
# Stage 2 — Runtime Image
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Cloud Run listens on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Placeholder env vars (will be injected at runtime by Cloud Run)
ENV DB_HOST=""
ENV DB_USER=""
ENV DB_PASS=""
ENV DB_NAME="taskmanagerdb"
ENV CLOUD_SQL_CONNECTION_NAME=""

EXPOSE 8080

ENTRYPOINT ["dotnet", "taskmanager-back-repo-qas.dll"]
