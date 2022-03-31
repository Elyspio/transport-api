namespace Transport.Api.Abstractions.Interfaces.Services;

public interface IDatabaseUpdateService
{
    Task RefreshYearly(int year);
}