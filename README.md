Всем добра!
Этот проект является сервером для мобильного приложения CulinaryChest(https://github.com/DenisFrolkov/CulinaryChest) по созданию рецептов. Чтобы запустить данный сервер на своем ПК, вам нужно изменить некоторый код:

1. Необходимо скачать этот репозиторий.
2. Проверьте версию dotnet, написав команду в терминал проекта: dotnet --version.
2. Если версия dotnet не 7, установите dotnet версии 7 здесь(https://dotnet.microsoft.com/download/dotnet/7.0).
3. Проверьте версию dotnet ef, написав команду в терминал проекта:
4. Для установки впервые: dotnet tool install --global dotnet-ef --version 7.0.16.
5. Для обновления: dotnet tool update --global dotnet-ef --version 7.0.16.
6. Запустите проект в Rider (https://www.jetbrains.com/ru-ru/rider/).
6. Измените файл appsettings.json:
Для PostgreSQL:
{
   "Logging": { "LogLevel": { "Default": "Warning" } },
   "ConnectionStrings": {
     "sqlConnection": "Host=localhost;Database=culinarychest-api;Integrated Security=true;TrustServerCertificate=True"
   },
   "JwtSettings": {
     "SecretKey": "CulinaryChestSecretKey12345678901234567890",
     "validIssuer": "CulinaryChestAPI",
     "validAudience": "https://localhost:7286",
     "expires": 5
   },
  "AllowedHosts": "*"
}
Для SSMS:
{
   "Logging": { "LogLevel": { "Default": "Warning" } },
   "ConnectionStrings": {
     "sqlConnection": "server=название_сервера;Database=culinarychest-api;Integrated Security=true;TrustServerCertificate=True"
   },
   "JwtSettings": {
     "SecretKey": "CulinaryChestSecretKey12345678901234567890",
     "validIssuer": "CulinaryChestAPI",
     "validAudience": "https://localhost:7286",
     "expires": 5
   },
  "AllowedHosts": "*"
}
7. Измените метод ConfigureSqlContext в классе ServiceExtensions:
Для PostgreSQL:
public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) => 
   services.AddDbContext<RepositoryContext>(opts => 
       opts.UseNpgsql(configuration.GetConnectionString("sqlConnection"), b => 
           b.MigrationsAssembly("culinarychest-api")));
Для SSMS:
public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
   services.AddDbContext<RepositoryContext>(opts => 
       opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
           b.MigrationsAssembly("culinarychest-api")));
8. Также вам нужно иметь сервер для запуска проекта:
    - Для PostgreSQL: https://postgresapp.com/downloads.html.
    - Для SSMS: https://learn.microsoft.com/ru-ru/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16.
10. Запускайте скачанный сервер.
11. Перестройте проект (нажатием на кнопку Build или Rebuild).
12. Сделайте миграцию базы данных в приложении Rider (Tools -> Entity Framework Core -> Add Migration). Имя миграции задавайте на свое усмотрение, DbContext class выбирайте RepositoryContext.
13. Обновите базу данных. Если делаете это впервые, база данных создастся на сервере (Tools -> Entity Framework Core -> Update Database). DbContext class выбирайте RepositoryContext.
14. Запустите сервер.
15. База данных должна добавиться на сервер, и можно делать запросы по адресу: https://localhost:7286/swagger/index.html.

Если что-то не получилось, это не ваша вина, а моя, как создателя проекта. Или просто напишите мне — Ден: https://t.me/o2232
