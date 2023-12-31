namespace YourBrand.Catalog.API.Features.ProductManagement;

using System;


public class VariantAlreadyExistsException : Exception
{
    public VariantAlreadyExistsException(string message) : base(message) { }
}