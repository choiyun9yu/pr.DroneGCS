FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
LABEL authors="choiyungyu"

# Compile
WORKDIR /gcs-system
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /gcs-system
COPY --from=build-env /kisa-gcs-dotnet/out .
ENTRYPOINT ["dotnet", "gcs-system.dll"]


