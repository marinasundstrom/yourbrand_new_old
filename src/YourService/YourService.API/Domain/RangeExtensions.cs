namespace YourBrand.YourService.API.Domain;

public static class RangeExtensions
{
    public static bool AddRange<T>(this HashSet<T> source, IEnumerable<T> items)
    {
        bool allAdded = true;
        foreach (T item in items)
        {
            allAdded &= source.Add(item);
        }
        return allAdded;
    }
}