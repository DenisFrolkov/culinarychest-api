using culinarychest_api.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

namespace culinarychest_api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        //Принимает объект IConfiguration, который используется для доступа к настройкам приложения.
        //В конструкторе загружается конфигурация для логирования с помощью NLog, используя файл nlog.config,
        //расположенный в корневом каталоге приложения.
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; } //это свойство, которое хранит объект IConfiguration. Этот объект используется для доступа к настройкам приложения

    public void ConfigureServices(IServiceCollection services) //это метод, где вы регистрируете и настраиваете сервисы, которые будут использоваться в вашем приложении. В этом методе:
    {
        services.ConfigureCors(); //вызывает метод расширения ConfigureCors, который настраивает CORS для приложения
        services.ConfigureIISIntegration(); //вызывает метод расширения ConfigureIISIntegration, предназначенный для настройки интеграции с IIS
        services.ConfigureLoggerService(); //предполагает вызов метода расширения для настройки сервиса логирования.
        services.AddControllers(); //предполагает вызов метода расширения для настройки сервиса логирования.
        services.ConfigureSqlContext(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { //это метод, где вы настраиваете конвейер обработки HTTP-запросов. В этом методе:
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); //Если приложение работает в режиме разработки (env.IsDevelopment()), включается страница исключений разработчика
        }
        
        app.UseHttpsRedirection(); //перенаправляет все HTTP-запросы на HTTPS
        app.UseHsts(); //добавляет заголовок HSTS для усиления безопасности
        app.UseStaticFiles(); //позволяет использовать статические файлы
        app.UseCors("CorsPolicy"); //позволяет использовать статические файлы
        app.UseForwardedHeaders(new ForwardedHeadersOptions //настраивает обработку заголовков X-Forwarded-For, X-Forwarded-Proto, X-Forwarded-Host, используемых при размещении приложения за обратным прокси
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        //UseRouting и UseAuthorization настраивают маршрутизацию и авторизацию соответственно.
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); //настраивает конечные точки приложения, включая маршруты, определенные в контроллерах.
        });
    }
}