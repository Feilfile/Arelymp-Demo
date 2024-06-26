# Building Image with SKD/Runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buildimage

# Update root CA to ensure outbound HTTPS requests don't fail
RUN apt-get update && \
    apt-get install -y ca-certificates && \
    apt-get clean && \
    update-ca-certificates

# Publish App for linux-x64
COPY . /app
RUN dotnet publish -c Release -r linux-x64 ./app/Frontend.csproj

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set Listen PORT https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/#environment-variables
ENV ASPNETCORE_URLS=http://*:51504

# Get publish from BuildImage
COPY --from=buildimage /app/bin/Release/net8.0/linux-x64/publish /app

WORKDIR /app

ENTRYPOINT ["./Frontend"]