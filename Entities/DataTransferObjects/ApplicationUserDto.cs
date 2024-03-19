namespace Entities.DataTransferObjects;

public class ApplicationUserDto
{
    //Класс ApplicationUserDto представляет собой Data Transfer Object (DTO),
    //который используется для передачи данных между слоями приложения.
    //DTO обычно используется для упрощения сложных объектов до простых структур данных,
    //которые легко передавать через сеть или между слоями приложения.
    public int UserId { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    //DTO, такие как ApplicationUserDto, часто используются в API для передачи данных между клиентом и сервером.
    //Например, при получении данных о пользователе, сервер может преобразовать объект ApplicationUser (который может
    //содержать дополнительные свойства и методы, не нужные клиенту) в ApplicationUserDto и отправить его обратно клиенту.
    //Это упрощает процесс передачи данных и помогает скрыть внутреннюю структуру данных от клиента.
}