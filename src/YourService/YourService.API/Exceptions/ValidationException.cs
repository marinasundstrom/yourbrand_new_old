﻿namespace YourBrand.YourService.API.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(Dictionary<string, string[]> errors)
    {
        Errors = errors;
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}