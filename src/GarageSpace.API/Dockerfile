FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src

COPY ["src/GarageSpace.API/GarageSpace.API.csproj", "GarageSpace.API/GarageSpace.API.csproj"]
COPY ["src/GarageSpaceAPI.Contracts/GarageSpaceAPI.Contracts.csproj", "GarageSpaceAPI.Contracts/GarageSpaceAPI.Contracts.csproj"]
COPY ["src/GarageSpace.Models/GarageSpace.Models.csproj", "GarageSpace.Models/GarageSpace.Models.csproj"]
COPY ["src/GarageSpace.Repository.EntityFramework/GarageSpace.Repository.EntityFramework.csproj", "GarageSpace.Repository.EntityFramework/GarageSpace.Repository.EntityFramework.csproj"]
COPY ["src/GarageSpace.Repository.Interfaces/GarageSpace.Repository.Interfaces.csproj", "GarageSpace.Repository.Interfaces/GarageSpace.Repository.Interfaces.csproj"]
COPY ["src/GarageSpace.Services/GarageSpace.Services.csproj", "GarageSpace.Services/GarageSpace.Services.csproj"]
COPY ["src/GarageSpace.Services.Interfaces/GarageSpace.Services.Interfaces.csproj", "GarageSpace.Services.Interfaces/GarageSpace.Services.Interfaces.csproj"]

RUN dotnet restore "GarageSpace.API/GarageSpace.API.csproj"
COPY ["/src", "."]
WORKDIR "/src/GarageSpace.API"
RUN dotnet build "GarageSpace.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GarageSpace.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GarageSpace.API.dll"]