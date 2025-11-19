# ==============================
# Stage 1 — Build and Publish
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore taskmanager-back-repo-qas.csproj
RUN dotnet build "./taskmanager-back-repo-qas.csproj" -c Debug -o /out

FROM build AS publish
RUN dotnet publish taskmanager-back-repo-qas.csproj -c Debug -o /out

# Building final image used in running container
FROM base AS final
RUN apk update \
    && apk add unzip procps
WORKDIR /src
COPY --from=publish /out .

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