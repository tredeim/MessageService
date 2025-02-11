FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MessageService.csproj", "./"]
RUN dotnet restore "MessageService.csproj"
COPY . .

RUN dotnet publish "MessageService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MessageService.dll"]