FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /source

COPY . ./

RUN dotnet restore ./PenguinCS.Login/PenguinCS.Login.csproj

RUN dotnet publish ./PenguinCS.Login/PenguinCS.Login.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:9.0

WORKDIR /app

COPY --from=build-env /source/out .

ENTRYPOINT [ "dotnet", "PenguinCS.Login.dll" ]