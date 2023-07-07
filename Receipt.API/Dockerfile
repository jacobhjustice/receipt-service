FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Receipt.API/Receipt.API.csproj", "Receipt.API/"]
COPY ["Receipt.API.DTOs/Receipt.API.DTOs.csproj", "Receipt.API.DTOs/"]
COPY ["Receipt.API.Logic/Receipt.API.Logic.csproj", "Receipt.API.Logic/"]
COPY ["Receipt.Models.Storage/Receipt.Models.Storage.csproj", "Receipt.Models.Storage/"]
COPY ["Receipt.Models/Receipt.Models.csproj", "Receipt.Models/"]
COPY ["Receipt.API.DTOs.Validators/Receipt.API.DTOs.Validators.csproj", "Receipt.API.DTOs.Validators/"]
RUN dotnet restore "Receipt.API/Receipt.API.csproj"
COPY . .
WORKDIR "/src/Receipt.API"
RUN dotnet build "Receipt.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Receipt.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Receipt.API.dll"]
