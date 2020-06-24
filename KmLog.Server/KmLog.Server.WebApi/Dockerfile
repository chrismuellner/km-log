FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["KmLog.Server.WebApi/KmLog.Server.WebApi.csproj", "KmLog.Server.WebApi/"]
COPY ["KmLog.Server.Logic/KmLog.Server.Logic.csproj", "KmLog.Server.Logic/"]
COPY ["KmLog.Server.Dal/KmLog.Server.Dal.csproj", "KmLog.Server.Dal/"]
COPY ["KmLog.Server.EF/KmLog.Server.EF.csproj", "KmLog.Server.EF/"]
COPY ["KmLog.Server.Model/KmLog.Server.Model.csproj", "KmLog.Server.Model/"]
COPY ["KmLog.Server.Domain/KmLog.Server.Domain.csproj", "KmLog.Server.Domain/"]
COPY ["KmLog.Server.Dto/KmLog.Server.Dto.csproj", "KmLog.Server.Dto/"]
COPY ["KmLog.Server.Blazor/KmLog.Server.Blazor.csproj", "KmLog.Server.Blazor/"]
RUN dotnet restore "KmLog.Server.WebApi/KmLog.Server.WebApi.csproj"
COPY . .
WORKDIR "/src/KmLog.Server.WebApi"
RUN dotnet build "KmLog.Server.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KmLog.Server.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KmLog.Server.WebApi.dll"]