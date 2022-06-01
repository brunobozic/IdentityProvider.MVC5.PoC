#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Identity.Vanilla.Mvc/Identity.Vanilla.Mvc.csproj", "src/Identity.Vanilla.Mvc/"]
RUN dotnet restore "src/Identity.Vanilla.Mvc/Identity.Vanilla.Mvc.csproj"
COPY . .
WORKDIR "/src/src/Identity.Vanilla.Mvc"
RUN dotnet build "Identity.Vanilla.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Vanilla.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Vanilla.Mvc.dll"]