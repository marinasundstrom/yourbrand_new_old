﻿namespace Sales.API.Features.OrderManagement.Domain.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string title)
    {
        Title = title;
    }

    public string Title { get; }
}