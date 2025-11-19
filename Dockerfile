# ==============================
# Stage 1 — Build and Publish
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy full source code and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ==============================
# Stage 2 — Runtime Image
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Cloud Run uses port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Environment variables that Cloud Run / Cloud SQL will send
ENV DB_HOST=""
ENV DB_USER=""
ENV DB_PASS=""
ENV DB_NAME=""

# Expose application port
EXPOSE 8080

# IMPORTANT: Replace dll name below with your actual output dll
ENTRYPOINT ["dotnet", "taskmanager-back-repo-qas.dll"]
