namespace YourBrand.YourService.API;

public static class Results
{
    public readonly static Result Success = new();
}

public class Result
{
    private readonly Error? error;

    internal Result()
    {
    }

    protected Result(Error? error)
    {
        this.error = error;
    }

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);

    public static Result<T> Success<T>(T data) => new(data);

    public static Result<T> Failure<T>(Error error) => new(error);

    public bool IsSuccess => error is null;

    public bool IsFailure => error is not null;

    public bool MatchError(Error error) => IsFailure && this.error == error;

    public bool MatchError<T>() where T : Error => this.error is T;

    public Error? GetError() => error;

    public T? GetError<T>() where T : Error => (T?)error;

    public static implicit operator Error(Result result) =>
        !result.IsFailure
        ? throw new InvalidOperationException() : result.error!;


    public static implicit operator Result(Error error) =>
        Result.Failure(error);

    public bool HasError(out Error error)
    {
        error = this.error!;
        return error is not null;
    }

    public bool HasError<T>(out T? error)
        where T : Error
    {
        if (this.error is T e)
        {
            error = e;
            return true;
        }

        error = null;
        return false;
    }
}

public class Result<T> : Result
{
    private T? data { get; }

    public Result(T data) : base(null)
    {
        this.data = data;
    }

    public Result(Error error) : base(error: error)
    {
    }

    public T GetValue() => data!;

    public static implicit operator T(Result<T> result) =>
        result.IsFailure
        ? throw new InvalidOperationException() : result.data!;

    public static implicit operator Result<T>(T result) =>
        Result.Success(result);

    public static implicit operator Result<T>(Error error) =>
        Result.Failure<T>(error);

    public bool HasSucceeded(out T result)
    {
        if (IsSuccess)
        {
            result = data!;
            return true;
        }

        result = default!;
        return false;
    }

    public R Match<R>(Func<T, R> success, Func<Error, R> error)
    {
        if (IsSuccess)
        {
            return success(GetValue());
        }

        return error(GetError()!);
    }

    public async Task<R> MatchAsync<R>(Func<T, Task<R>> success, Func<Error, Task<Result<R>>> error)
    {
        if (IsSuccess)
        {
            return await success(GetValue());
        }

        return await error(GetError()!);
    }
}