using ConfigurationDemo.Configurations;
using ConfigurationDemo.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configurationPrefix = $"DemoConfiguration:{builder.Environment.EnvironmentName}";

builder.Services.AddControllers();

// Adding Azure AppConfiguration + Azure Key Vault as an external configuration store
builder.Configuration.AddExternalConfigurationStoreServices(configurationPrefix);

// add required services for the middleware for refresh from Azure App Configuration
builder.Services.AddAzureAppConfiguration();

builder.Services.Configure<CosmosDbConfiguration>(builder.Configuration.GetSection(CosmosDbConfiguration.SectionName));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// add this middleware for refresh from Azure App Configuration
app.UseAzureAppConfiguration();

app.MapControllers();

app.Run();
