FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AvatarNavigator.API.dll"]
