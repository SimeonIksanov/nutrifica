FROM nginx AS base
EXPOSE 80
EXPOSE 443
ENV BACKEND_SCHEME=http
ENV BACKEND_AUTHORITY=localhost:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Instal wasm-tools
RUN dotnet workload install wasm-tools

# Install python
RUN apt-get update && apt-get install -yq \
    ca-certificates \
    curl \
    gnupg \
    python3

COPY ./*.sln src/*/*.csproj ./
RUN for file in $(ls *.csproj); \
    do \
        mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; \
    done

# Restore
RUN dotnet restore ./src/Nutrifica.Spa/Nutrifica.Spa.csproj

COPY ./src ./src

# Build
RUN dotnet build ./src/Nutrifica.Spa/Nutrifica.Spa.csproj -c Release --no-restore

# Publish
RUN dotnet publish ./src/Nutrifica.Spa/Nutrifica.Spa.csproj -c Release --no-restore -o published

FROM  base AS final
WORKDIR /usr/share/nginx/html
COPY --chmod=555 ./change_backend_url.sh /docker-entrypoint.d/40-change_backend_url.sh
COPY --from=build-env /src/published/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf