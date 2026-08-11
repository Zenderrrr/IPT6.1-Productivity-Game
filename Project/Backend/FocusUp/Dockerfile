FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

# development build
FROM base AS development

EXPOSE 8080

CMD ["dotnet", "watch", "run", "--project", "FocusUp.csproj", "--urls=http://0.0.0.0:8080"]

# production build
FROM base AS build

RUN dotnet publish FocusUp.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false


# production server
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS production

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

RUN mkdir -p /app/Data && chown -R app:app /app/Data
USER app

ENTRYPOINT ["dotnet", "FocusUp.dll"]