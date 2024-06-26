#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Update root CA to ensure outbound HTTPS requests don't fail
RUN apt-get update && \
    apt-get install -y ca-certificates && \
    update-ca-certificates && \
    apt-get clean

# Publish App for linux-x64
COPY . /app
COPY ../Configuration/Configuration.csproj ./Configuration/

RUN dotnet publish -c Release -r linux-x64 /app/Director.csproj

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Get publish from BuildImage
COPY --from=build /app/bin/Release/net8.0/linux-x64/publish /app

WORKDIR /app

ENTRYPOINT ["./director"]   