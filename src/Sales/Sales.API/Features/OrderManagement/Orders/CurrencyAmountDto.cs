using System.Linq.Expressions;
using System.Reflection;

namespace Sales.API.Features.OrderManagement.Orders;

public record CurrencyAmountDto(string Currency, decimal Amount);