#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["IdentityProvider.UI.Web.MVC6/IdentityProvider.UI.Web.MVC6.csproj", "IdentityProvider.UI.Web.MVC6/"]
RUN dotnet restore "IdentityProvider.UI.Web.MVC6/IdentityProvider.UI.Web.MVC6.csproj"
COPY . .
WORKDIR "/src/IdentityProvider.UI.Web.MVC6"
RUN dotnet build "IdentityProvider.UI.Web.MVC6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityProvider.UI.Web.MVC6.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityProvider.UI.Web.MVC6.dll"]