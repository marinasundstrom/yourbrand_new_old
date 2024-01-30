namespace YourBrand.YourService.API.Domain;

public static class Errors
{
    public static readonly Error SearchTermIsEmpty = new Error(nameof(SearchTermIsEmpty), "Search term is empty", string.Empty);

    public static class Todos
    {
        public static readonly Error TodoNotFound = new Error(nameof(TodoNotFound), "Todo not found", string.Empty);
    }

    public static class Users
    {
        public static readonly Error UserNotFound = new Error(nameof(UserNotFound), "User not found", string.Empty);
        public static readonly Error TodoNotFound = new Error(nameof(TodoNotFound), "Todo not found", string.Empty);
    }
}