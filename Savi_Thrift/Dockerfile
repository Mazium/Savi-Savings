#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7226
EXPOSE 5226

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Savi_Thrift/Savi_Thrift.csproj", "Savi_Thrift/"]
COPY ["Savi_Thrift.Common/Savi_Thrift.Common.csproj", "Savi_Thrift.Common/"]
COPY ["Savi_Thrift.Domain/Savi_Thrift.Domain.csproj", "Savi_Thrift.Domain/"]
COPY ["Savi_Thrift.Persistence/Savi_Thrift.Persistence.csproj", "Savi_Thrift.Persistence/"]
COPY ["Savi_Thrift.Application/Savi_Thrift.Application.csproj", "Savi_Thrift.Application/"]
RUN dotnet restore "./Savi_Thrift/./Savi_Thrift.csproj"
COPY . .
WORKDIR "/src/Savi_Thrift"
RUN dotnet build "./Savi_Thrift.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Savi_Thrift.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Savi_Thrift.dll"]