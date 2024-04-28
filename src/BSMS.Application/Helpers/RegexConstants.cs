namespace BSMS.Application.Helpers;

public static class RegexConstants
{
    public const string LettersOnly = @"^[a-zA-Z]+$";
    public const string LettersAndNumbers = @"^[a-zA-Z0-9]+$";
    public const string PhoneNumber = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";
    public const string Email = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
    public const string DriverLicense = @"^[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}$";
    public const string Street = @"^[a-zA-Z0-9\s]+(?:-[a-zA-Z0-9\s]+)?$";
    public const string City = @"^[A-Za-z\s.-]+$";
    public const string ZipCode = @"\d{5}(?:-?\d{4})?";
}