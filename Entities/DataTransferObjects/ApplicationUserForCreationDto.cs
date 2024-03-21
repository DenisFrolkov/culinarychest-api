namespace Entities.DataTransferObjects;

public class ApplicationUserForCreationDto
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}