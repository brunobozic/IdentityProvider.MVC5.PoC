#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/IdentityProvider.Web.MVC6/IdentityProvider.Web.MVC6.csproj", "src/IdentityProvider.Web.MVC6/"]
RUN dotnet restore "src/IdentityProvider.Web.MVC6/IdentityProvider.Web.MVC6.csproj"
COPY . .
WORKDIR "/src/src/IdentityProvider.Web.MVC6"
RUN dotnet build "IdentityProvider.Web.MVC6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityProvider.Web.MVC6.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityProvider.Web.MVC6.dll"]