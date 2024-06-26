# Building Image with SKD/Runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS buildimage

# Publish App for linux-x64
COPY . /app
RUN dotnet publish -c Release -r linux-x64 /app/MatchFunction.csproj

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set Listen PORT https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/#environment-variables
ENV ASPNETCORE_URLS=http://*:51502

# Get publish from BuildImage
COPY --from=buildimage /app/bin/Release/net8.0/linux-x64/publish /app

WORKDIR /app

ENTRYPOINT ["./MatchFunction"]