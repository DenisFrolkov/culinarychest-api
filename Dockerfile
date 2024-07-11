FROM mcr.microsoft.com/dotnet/sdk:7.0

# Установка PostgreSQL клиента для доступа к локальной БД
RUN apt-get update && apt-get install -y postgresql-client

# Установка .NET Core CLI tools
RUN dotnet tool install --global dotnet-ef

WORKDIR /app
COPY . .

CMD ["dotnet", "run"]
