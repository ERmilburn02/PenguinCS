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

WORKDIR /app

# Use a non-root user for security
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build-env /out .

EXPOSE 9912

ENTRYPOINT [ "dotnet", "PenguinCS.Login.dll" ]
