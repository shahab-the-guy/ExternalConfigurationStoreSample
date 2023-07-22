using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace ConfigurationDemo.Extensions;

internal static class ExternalConfigurationStoreExtensions
{
    internal static void AddExternalConfigurationStoreServices(
        this ConfigurationManager configuration, string configurationPrefix)
    {
        ArgumentException.ThrowIfNullOrEmpty(configurationPrefix);

        /*
         *  This section is not needed after connecting
         *  AppConfiguration to the Key Vault
         *  Check out Lines 44-50
        */
        // if (configuration.GetConnectionString("VaultUri") is { } vaultUrl)
        // {
        //     configuration.AddAzureKeyVault(new Uri(vaultUrl), new DefaultAzureCredential(),
        //         new AzureKeyVaultConfigurationOptions()
        //         {
        //             ReloadInterval = TimeSpan.FromSeconds(5),
        //             Manager = new SamplePrefixKeyVaultSecretManager(configurationPrefix)
        //         });
        // }

        if (configuration.GetConnectionString("AppConfiguration") is { } appConfigurationUri)
        {
            configuration.AddAzureAppConfiguration(options =>
            {
                var credentialToken = new DefaultAzureCredential();

                var appConfigurationConnection = options.Connect(new Uri(appConfigurationUri), credentialToken)
                        .Select($"{configurationPrefix}:*")
                        .TrimKeyPrefix($"{configurationPrefix}:")
                        .ConfigureRefresh(refresherConfiguration =>
                        {
                            refresherConfiguration.Register($"{configurationPrefix}:Sentinel", refreshAll: true)
                                .SetCacheExpiration(TimeSpan.FromSeconds(5));
                        })
                    ;

                if (configuration.GetConnectionString("VaultUri") is { } vaultUri)
                    appConfigurationConnection
                        .ConfigureKeyVault(keyVaultOptions =>
                        {
                            var secretClient = new SecretClient(new Uri(vaultUri), credentialToken);
                            keyVaultOptions.Register(secretClient);
                        });
            });
        }
    }
}
