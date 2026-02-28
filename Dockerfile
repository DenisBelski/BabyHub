FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["BabyHub/BabyHub.csproj", "BabyHub/"]
COPY ["BabyHub.HttpApi/BabyHub.HttpApi.csproj", "BabyHub.HttpApi/"]
COPY ["BabyHub.Application/BabyHub.Application.csproj", "BabyHub.Application/"]
COPY ["BabyHub.Application.Contracts/BabyHub.Application.Contracts.csproj", "BabyHub.Application.Contracts/"]
COPY ["BabyHub.Domain/BabyHub.Domain.csproj", "BabyHub.Domain/"]
COPY ["BabyHub.Domain.Shared/BabyHub.Domain.Shared.csproj", "BabyHub.Domain.Shared/"]
COPY ["BabyHub.EntityFrameworkCore/BabyHub.EntityFrameworkCore.csproj", "BabyHub.EntityFrameworkCore/"]

RUN dotnet restore "BabyHub/BabyHub.csproj"

COPY . .
WORKDIR "/src/BabyHub"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BabyHub.dll"]