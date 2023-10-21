using System;
using System.Globalization;

using MassTransit.Transports;

namespace Catalog.API.Features.ProductManagement.Products;

public static class Errors
{
    public readonly static Error ProductNotFound = new("product-not-found", "Product not found", "");

    public readonly static Error HandleAlreadyTaken = new("handle-already-taken", "Handle already taken", "");
}