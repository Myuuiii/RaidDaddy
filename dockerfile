# Establish working directory
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the source code to the build context
COPY . ./
RUN dotnet restore
RUN dotnet publish RaidDaddy -c Release -o out

# Build Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
VOLUME [ "/app/resource" ]
ENTRYPOINT ["dotnet", "RaidDaddy.dll"]