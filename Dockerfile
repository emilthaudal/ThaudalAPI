FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/ThaudalAPI/ThaudalAPI.csproj", "ThaudalAPI/"]
COPY ["/Modules/DayOfWeekService/DayOfWeekService.csproj", "DayOfWeekService/"]
COPY ["/Modules/TodoService/TodoService.csproj", "TodoService/"]
COPY ["/ThaudalAPI.Model/ThaudalAPI.Model.csproj", "ThaudalAPI.Model/"]
COPY ["/Modules/UserService/UserService.csproj", "UserService/"]
RUN dotnet restore "ThaudalAPI/ThaudalAPI.csproj"
COPY . .
WORKDIR "/src/ThaudalAPI"
RUN dotnet build "ThaudalAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThaudalAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThaudalAPI.dll"]
