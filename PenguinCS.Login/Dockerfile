# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /source

# Copy and restore dependencies (better caching)
COPY ./PenguinCS.Common/PenguinCS.Common.csproj ./PenguinCS.Common/
COPY ./PenguinCS.Data/PenguinCS.Data.csproj ./PenguinCS.Data/
COPY ./PenguinCS.Login/PenguinCS.Login.csproj ./PenguinCS.Login/
RUN dotnet restore ./PenguinCS.Login/PenguinCS.Login.csproj

# Copy remaining source and build
COPY . ./
RUN dotnet publish ./PenguinCS.Login/PenguinCS.Login.csproj -c Release -o /out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/runtime:9.0

LABEL org.opencontainers.image.source=https://github.com/ERmilburn02/PenguinCS
LABEL org.opencontainers.image.description="PenguinCS Login Server"
LABEL org.opencontainers.image.licenses=MIT

WORKDIR /app

# Use a non-root user for security (built-in to the base image)
USER app

COPY --from=build-env /out .

EXPOSE 9912

ENTRYPOINT [ "dotnet", "PenguinCS.Login.dll" ]
