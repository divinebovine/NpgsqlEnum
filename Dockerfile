FROM mcr.microsoft.com/dotnet/sdk:7.0-bookworm-slim

WORKDIR /app

COPY NpgsqlEnum.csproj ./

RUN dotnet restore

COPY . ./

RUN dotnet publish -o ./out ./NpgsqlEnum.csproj

ENTRYPOINT [ "dotnet", "./out/NpgsqlEnum.dll" ]
