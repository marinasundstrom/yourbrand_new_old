using System.Linq.Expressions;
using System.Reflection;

namespace YourBrand.YourService.API.Features.OrderManagement.Orders;

public record CurrencyAmountDto(string Currency, decimal Amount);