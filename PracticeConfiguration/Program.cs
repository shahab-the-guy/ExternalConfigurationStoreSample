using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using PracticeConfiguration.Configurations;

const string applicationName = "DemoConfiguration";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

if (builder.Configuration.GetConnectionString("AppConfiguration") is { } appConfigurationUri)
{
    var configurationPrefix = $"{applicationName}:{builder.Environment.EnvironmentName}"; 
    builder.Configuration.AddAzureAppConfiguration(options =>
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

        if (builder.Configuration.GetConnectionString("VaultUri") is { } vaultUri)
            appConfigurationConnection
                .ConfigureKeyVault(keyVaultOptions =>
                {
                    var secretClient = new SecretClient(new Uri(vaultUri), credentialToken);
                    keyVaultOptions.Register(secretClient);
                })
                ;
    });
}

// add required services for the middleware for refresh from Azure App Configuration
builder.Services.AddAzureAppConfiguration();

builder.Services.Configure<CosmosDbConfiguration>(builder.Configuration.GetSection(CosmosDbConfiguration.SectionName));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// add this middleware for refresh from Azure App Configuration
app.UseAzureAppConfiguration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
