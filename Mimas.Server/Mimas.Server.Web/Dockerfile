#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mimas.Server.Web/Mimas.Server.Web.csproj", "Mimas.Server.Web/"]
RUN dotnet restore "Mimas.Server.Web/Mimas.Server.Web.csproj"
COPY . .
WORKDIR "/src/Mimas.Server.Web"
RUN dotnet build "Mimas.Server.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mimas.Server.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mimas.Server.Web.dll"]