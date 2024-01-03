namespace YourBrand.YourService.API.Services;

public interface IDateTime
{
    DateTimeOffset Now { get; }
}