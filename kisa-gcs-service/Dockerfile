FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
LABEL authors="choiyungyu"

# Compile
WORKDIR /kisa-gcs-service
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /kisa-gcs-service
COPY --from=build-env /kisa-gcs-service/out .
ENTRYPOINT ["dotnet", "kisa-gcs-service.dll"]


