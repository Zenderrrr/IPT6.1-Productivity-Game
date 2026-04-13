FROM mcr.microsoft.com/dotnet/sdk:10.0-preview

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

EXPOSE 8080

CMD ["dotnet", "watch", "run", "--project", "FocusUp.csproj", "--urls=http://0.0.0.0:8080"]