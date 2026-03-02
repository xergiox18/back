FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ClasificationProjects.Opi.Back.sln ./

COPY Core/ClasificationProjects.Opi.Back.Domain/ClasificationProjects.Opi.Back.Domain.csproj Core/ClasificationProjects.Opi.Back.Domain/
COPY Core/ClasificationProjects.Opi.Back.Application/ClasificationProjects.Opi.Back.Application.csproj Core/ClasificationProjects.Opi.Back.Application/
COPY Infrastructure/ClasificationProjects.Opi.Back.Infrastructure/ClasificationProjects.Opi.Back.Infrastructure.csproj Infrastructure/ClasificationProjects.Opi.Back.Infrastructure/
COPY Presentation/ClasificationProjects.Opi.Back.Api/ClasificationProjects.Opi.Back.Api.csproj Presentation/ClasificationProjects.Opi.Back.Api/

RUN dotnet restore Presentation/ClasificationProjects.Opi.Back.Api/ClasificationProjects.Opi.Back.Api.csproj

COPY . ./
RUN dotnet publish Presentation/ClasificationProjects.Opi.Back.Api/ClasificationProjects.Opi.Back.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ClasificationProjects.Opi.Back.Api.dll"]
