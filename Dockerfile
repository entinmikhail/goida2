FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY goida.sln .
COPY goida/goida.csproj goida/
RUN dotnet restore
COPY goida/ goida/
RUN dotnet publish goida/goida.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "goida.dll"]
