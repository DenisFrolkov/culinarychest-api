using Contracts;
using culinarychest_api.ActionFilters;
using culinarychest_api.Extensions;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository;
using Repository.DataShaping;

namespace culinarychest_api;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        //Принимает объект IConfiguration, который используется для доступа к настройкам приложения.
        //В конструкторе загружается конфигурация для логирования с помощью NLog, используя файл nlog.config,
        //расположенный в корневом каталоге приложения.
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        Configuration = configuration;
    }

    public IWebHostEnvironment HostEnvironment { get; }
    public IConfiguration Configuration { get; } //это свойство, которое хранит объект IConfiguration. Этот объект используется для доступа к настройкам приложения

    public void ConfigureServices(IServiceCollection services) //это метод, где вы регистрируете и настраиваете сервисы, которые будут использоваться в вашем приложении. В этом методе:
    {
        services.ConfigureCors(); //вызывает метод расширения ConfigureCors, который настраивает CORS для приложения
        services.ConfigureIISIntegration(); //вызывает метод расширения ConfigureIISIntegration, предназначенный для настройки интеграции с IIS
        services.ConfigureLoggerService(); //предполагает вызов метода расширения для настройки сервиса логирования
        services.ConfigureRepositoryManager(); //регистрирует RepositoryManager как реализацию интерфейса IRepositoryManager в контейнере зависимостей.
        services.AddControllers(); //предполагает вызов метода расширения для настройки сервиса логирования
        services.ConfigureSqlContext(Configuration); //предполагает вызов метода расширения для настройки контекста базы данных SQL
        services.AddAutoMapper(typeof(Startup));
        services.AddControllers(configure =>
            {
                configure.RespectBrowserAcceptHeader = true;
                configure.ReturnHttpNotAcceptable = true;
            }
        );
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddScoped<ValidationFilterAttribute>();
        services.AddScoped <IDataShaper<RecipeDto>, DataShaper<RecipeDto>>();
        services.AddAuthentication(); 
        services.ConfigureIdentity();
        services.ConfigureJWT(Configuration);
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.ConfigureExceptionHandler(logger);
        app.UseHttpsRedirection();
        app.UseHsts();
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}