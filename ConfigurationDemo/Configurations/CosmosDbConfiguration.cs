namespace ConfigurationDemo.Configurations;

public sealed class CosmosDbConfiguration {
    
    public const string SectionName = "CosmosDB";
    
    public required string AccountEndpoint { get; init; }
    public required string DatabaseName { get; init; } 
    public required string CollectionName { get; init; }
    
    /// <summary>
    /// This is a secret and should be stored in either Azure Key Vault, or dotnet user-secrets for local development
    /// </summary>
    public required string AccountKey { get; init; }  
}
