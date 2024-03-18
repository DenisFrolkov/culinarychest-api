using System.Net;
using Contracts;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;

namespace culinarychest_api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    // Класс ExceptionMiddlewareExtensions представляет собой расширение для IApplicationBuilder,
    // которое позволяет настроить глобальную обработку исключений в ASP.NET Core приложении.
    // Это достигается с помощью использования встроенного middleware UseExceptionHandler.
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        // Метод ConfigureExceptionHandler добавляет middleware UseExceptionHandler в конвейер обработки запросов.
        // Этот middleware перехватывает все исключения, возникающие в приложении, и позволяет определить, как они должны быть обработаны.
     app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                //Внутри UseExceptionHandler, определяется асинхронный обработчик, который выполняется при возникновении исключения.
                //Этот обработчик устанавливает статус ответа на 500 (Internal Server Error), устанавливает тип контента ответа как
                //application/json, и, если доступна информация об исключении, логирует ошибку и отправляет пользователю сообщение
                //об ошибке в формате JSON
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                //Для доступа к информации об исключении используется интерфейс IExceptionHandlerFeature, который предоставляет
                //свойство Error, содержащее объект исключения.
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    //В случае возникновения исключения, обработчик создает экземпляр класса ErrorDetails, устанавливает в нем статус
                    //ответа и сообщение об ошибке, а затем сериализует этот объект в JSON и отправляет его в ответе
                    logger.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error."
                    }.ToString());
                }
            });
        });
    }
}