#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Assessment_BE_Engineer_3_IdentityAPI/Assessment_BE_Engineer_3_IdentityAPI.csproj", "Assessment_BE_Engineer_3_IdentityAPI/"]
COPY ["Assessment_BE_Engineer_3_Utility/Assessment_BE_Engineer_3_Utility.csproj", "Assessment_BE_Engineer_3_Utility/"]
RUN dotnet restore "Assessment_BE_Engineer_3_IdentityAPI/Assessment_BE_Engineer_3_IdentityAPI.csproj"
COPY . .
WORKDIR "/src/Assessment_BE_Engineer_3_IdentityAPI"
RUN dotnet build "Assessment_BE_Engineer_3_IdentityAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Assessment_BE_Engineer_3_IdentityAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assessment_BE_Engineer_3_IdentityAPI.dll"]