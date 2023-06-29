namespace PracticeConfiguration.Configurations;

public sealed class CosmosDbConfiguration {
    
    public const string SectionName = "CosmosDB";
    
    public required string AccountEndpoint { get; init; }
    public required string AccountKey { get; init; } 
    public required string DatabaseName { get; init; } 
    public required string CollectionName { get; init; }
}
