#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IDP.MVC6.Vanilla/IDP.MVC6.Vanilla.csproj", "src/IDP.MVC6.Vanilla/"]
RUN dotnet restore "src/IDP.MVC6.Vanilla/IDP.MVC6.Vanilla.csproj"
COPY . .
WORKDIR "/src/src/IDP.MVC6.Vanilla"
RUN dotnet build "IDP.MVC6.Vanilla.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IDP.MVC6.Vanilla.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDP.MVC6.Vanilla.dll"]