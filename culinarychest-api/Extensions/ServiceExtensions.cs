using Contracts;
using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;

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
    
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts => 
            opts.UseNpgsql(configuration.GetConnectionString("sqlConnection"), b => 
                b.MigrationsAssembly("culinarychest-api")));

}