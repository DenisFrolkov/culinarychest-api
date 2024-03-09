using culinarychest_api;

public class Program
{
    public static void Main(string[] args) //Это точка входа в приложение. Когда приложение запускается, метод Main вызывается первым. В этом методе создается и запускается хост приложения.
    {
        CreateHostBuilder(args).Build().Run(); //Этот метод создает и настраивает объект IHostBuilder, который используется для настройки и запуска приложения
    }
    public static IHostBuilder CreateHostBuilder(string[] args) => //создает объект IHostBuilder с настройками по умолчанию, включая настройки для конфигурации, логирования, и других служб
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); //Этот метод настраивает веб-хост для приложения. Внутри него вызывается webBuilder.UseStartup<Startup>(), который указывает, что класс Startup будет использоваться для настройки сервисов и конвейера обработки запросов
            }
        );
}