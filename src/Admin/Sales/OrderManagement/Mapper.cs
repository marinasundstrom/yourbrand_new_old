using Microsoft.VisualBasic;

using YourBrand.Sales;

using static YourBrand.Admin.Sales.OrderManagement.OrderItemViewModel;

namespace YourBrand.Admin.Sales.OrderManagement;

public static class Mapper
{
    public static OrderViewModel ToModel(this YourBrand.Sales.Order dto)
    {
        var model = new OrderViewModel
        {
            Id = dto.OrderNo,
            Date = dto.Date.Date,
            Status = dto.Status,
        };

        foreach (var item in dto.Items)
        {
            model.AddItem(item.ToModel());
        }

        return model;
    }

    public static OrderItemViewModel ToModel(this OrderItem dto)
    {
        return new OrderItemViewModel
        {
            Id = dto.Id,
            Description = dto.Description,
            ItemId = dto.ItemId,
            Notes = dto.Notes,
            UnitPrice = dto.UnitPrice,
            Unit = dto.Unit ?? string.Empty,
            Quantity = dto.Quantity,
            VatRate = dto.VatRate.GetValueOrDefault(),
            Discount = dto.Discount
        };
    }

    public static AddOrderItemRequest ToCreateOrderItemRequest(this OrderItemViewModel vm)
    {
        return new AddOrderItemRequest
        {
            Description = vm.Description,
            ItemId = vm.ItemId,
            Notes = vm.Notes,
            UnitPrice = vm.UnitPrice,
            Unit = vm.Unit,
            Quantity = vm.Quantity,
            VatRate = vm.VatRate.GetValueOrDefault(),
            Discount = vm.Discount
        };
    }

    public static UpdateOrderItemRequest ToUpdateOrderItemRequest(this OrderItemViewModel dto)
    {
        return new UpdateOrderItemRequest
        {
            Description = dto.Description,
            ItemId = dto.ItemId,
            Notes = dto.Notes,
            UnitPrice = dto.UnitPrice,
            Unit = dto.Unit,
            Quantity = dto.Quantity,
            VatRate = dto.VatRate.GetValueOrDefault(),
        };
    }
}
