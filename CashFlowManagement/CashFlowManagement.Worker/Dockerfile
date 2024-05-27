#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:8.0-nanoserver-1809 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CashFlowManagement.Worker/CashFlowManagement.Worker.csproj", "CashFlowManagement.Worker/"]
COPY ["CashFlowManagement.CrossCutting/CashFlowManagement.CrossCutting.csproj", "CashFlowManagement.CrossCutting/"]
COPY ["CashFlowManagement.AntiCorruption/CashFlowManagement.AntiCorruption.csproj", "CashFlowManagement.AntiCorruption/"]
COPY ["CashFlowManagement.Domain/CashFlowManagement.Domain.csproj", "CashFlowManagement.Domain/"]
COPY ["CashFlowManagement.Application/CashFlowManagement.Application.csproj", "CashFlowManagement.Application/"]
COPY ["CashFlowManagement.RabbitMq/CashFlowManagement.RabbitMq.csproj", "CashFlowManagement.RabbitMq/"]
COPY ["CashFlowManagement.Repository/CashFlowManagement.Repository.csproj", "CashFlowManagement.Repository/"]
RUN dotnet restore "./CashFlowManagement.Worker/CashFlowManagement.Worker.csproj"
COPY . .
WORKDIR "/src/CashFlowManagement.Worker"
RUN dotnet build "./CashFlowManagement.Worker.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CashFlowManagement.Worker.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CashFlowManagement.Worker.dll"]