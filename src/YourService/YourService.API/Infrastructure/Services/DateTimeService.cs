namespace YourBrand.YourService.API.Infrastructure.Services;

sealed class DateTimeService : IDateTime
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}