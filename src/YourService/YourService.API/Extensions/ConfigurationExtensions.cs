using Azure.Identity;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddAzureConfiguration(this ConfigurationManager configurationManager, IConfiguration configuration)
    {
        configurationManager.AddAzureAppConfiguration(options =>
            options.Connect(
                new Uri($"https://{configuration["Azure:AppConfig:Name"]}.azconfig.io"),
                new DefaultAzureCredential()));

        configurationManager.AddAzureKeyVault(
            new Uri($"https://{configuration["Azure:KeyVault:Name"]}.vault.azure.net/"),
            new DefaultAzureCredential());

        return configurationManager;
    }
}
