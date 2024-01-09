using Asp.Versioning;

namespace YourBrand.YourService.API;

public static class ApiVersions
{
    public static readonly ApiVersion V1 = new ApiVersion(1, 0);

    static IEnumerable<ApiVersion>? _all;

    public static IEnumerable<ApiVersion> All => _all
        ??= typeof(ApiVersion)
        .GetFields(System.Reflection.BindingFlags.Static)
        .Where(x => x.FieldType == typeof(ApiVersion)).Select(x => (ApiVersion)x.GetValue(null)!)
        .ToList().AsEnumerable();
}