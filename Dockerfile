FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Directory.Build.props", "./"]
COPY ["RecoveryFiscal.sln", "./"]

COPY ["src/RecoveryFiscal.Api/RecoveryFiscal.Api.csproj", "src/RecoveryFiscal.Api/"]
COPY ["src/RecoveryFiscal.Application/RecoveryFiscal.Application.csproj", "src/RecoveryFiscal.Application/"]
COPY ["src/RecoveryFiscal.Domain/RecoveryFiscal.Domain.csproj", "src/RecoveryFiscal.Domain/"]
COPY ["src/RecoveryFiscal.Infrastructure/RecoveryFiscal.Infrastructure.csproj", "src/RecoveryFiscal.Infrastructure/"]

RUN dotnet restore "src/RecoveryFiscal.Api/RecoveryFiscal.Api.csproj"

COPY . .
RUN dotnet publish "src/RecoveryFiscal.Api/RecoveryFiscal.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RecoveryFiscal.Api.dll"]
