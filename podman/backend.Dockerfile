FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /Sources

# Copy
COPY ./*.sln src/*/*.csproj ./
RUN for file in $(ls *.csproj); \
    do \
        mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; \
    done

# Restore
RUN dotnet restore ./src/Nutrifica.Api/Nutrifica.Api.csproj

# Copy all the rest files
COPY ./src ./src

# Build
RUN dotnet build ./src/Nutrifica.Api/Nutrifica.Api.csproj -c Release --no-restore

# Publish
RUN dotnet publish ./src/Nutrifica.Api/Nutrifica.Api.csproj -c Release --no-restore -o /Published

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /Published .
ENTRYPOINT ["dotnet", "Nutrifica.Api.dll"]