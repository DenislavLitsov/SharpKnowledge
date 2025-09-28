# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SharpKnowledge.Learning/SharpKnowledge.Learning.csproj", "SharpKnowledge.Learning/"]
COPY ["SharpKnowledge.Common/SharpKnowledge.Common.csproj", "SharpKnowledge.Common/"]
COPY ["SharpKnowledge.Games.Snake/SharpKnowledge.Games.Snake.Engine/SharpKnowledge.Games.Snake.Engine.csproj", "SharpKnowledge.Games.Snake/SharpKnowledge.Games.Snake.Engine/"]
COPY ["SharpKnowledge.Playing/SharpKnowledge.Playing.csproj", "SharpKnowledge.Playing/"]
COPY ["SharpKnowledge.Knowledge/SharpKnowledge.Knowledge.csproj", "SharpKnowledge.Knowledge/"]
COPY ["SharpKnowledge.Data/SharpKnowledge.Data.csproj", "SharpKnowledge.Data/"]
RUN dotnet restore "./SharpKnowledge.Learning/SharpKnowledge.Learning.csproj"
COPY . .
WORKDIR "/src/SharpKnowledge.Learning"
RUN dotnet build "./SharpKnowledge.Learning.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SharpKnowledge.Learning.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final

USER root
RUN mkdir -p /app/data \
 && chown -R $APP_UID /app/data
USER $APP_UID

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SharpKnowledge.Learning.dll"]