namespace Domain.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException( string exceptionMessage ) : base( exceptionMessage ) { }
}
