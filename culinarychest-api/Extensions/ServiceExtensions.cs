using Contracts;
using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace culinarychest_api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) //это метод расширения, который настраивает CORS для вашего приложения
        => services.AddCors(options => //добавляет службы CORS в контейнер зависимостей вашего приложения
        {
            options.AddPolicy("CorsPolicy", builder => //создает новую политику CORS с именем "CorsPolicy". Внутри этой политики:
                builder.AllowAnyOrigin() //разрешает запросы от любого источника
                    .AllowAnyMethod() //разрешает использование любого HTTP-метода (GET, POST, PUT, DELETE и т.д.)
                    .AllowAnyHeader()); //разрешает использование любых заголовков в запросах
        }
    );
    public static void ConfigureIISIntegration(this IServiceCollection services) //это метод расширения, предназначенный для настройки интеграции с IIS
        => services.Configure<IISOptions>(options => { }); //позволяет настроить параметры, связанные с IIS, используя класс IISOptions
    
    public static void ConfigureLoggerService(this IServiceCollection services) //это метод расширения, который регистрирует пользовательский сервис логирования в контейнере зависимостей
        => services.AddScoped<ILoggerManager, LoggerManager>(); //добавляет LoggerManager как реализацию интерфейса ILoggerManager в контейнер зависимостей с областью видимости "Scoped
    
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) => //IConfiguration configuration: Этот параметр представляет конфигурацию приложения, которая содержит различные настройки, включая строки подключения к базе данных
        services.AddDbContext<RepositoryContext>(opts => //регистрирует RepositoryContext как сервис в контейнере внедрения зависимостей ASP.NET Core
            //AddDbContext, вы указываете, как DbContext должен быть настроен. В данном случае, используется UseNpgsql для указания, что DbContext должен использовать PostgreSQL в качестве базы данных
            opts.UseNpgsql(configuration.GetConnectionString("sqlConnection"), b => 
                b.MigrationsAssembly("culinarychest-api"))); //указывает, что сборка с миграциями находится в проекте culinarychest-api

    public static void ConfigureRepositoryManager(this IServiceCollection services) => 
        //метод расширения регистрирует RepositoryManager как реализацию интерфейса IRepositoryManager в контейнере зависимостей.
        //Это позволяет вашему приложению использовать RepositoryManager для централизованного управления репозиториями,
        //что упрощает тестирование и поддержку кода.
        services.AddScoped<IRepositoryManager, RepositoryManager>();

}