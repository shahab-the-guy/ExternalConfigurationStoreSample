using ConfigurationDemo.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly IOptionsSnapshot<CosmosDbConfiguration> _cosmosDbConfigurationOptions;

    public ConfigurationController(IOptionsSnapshot<CosmosDbConfiguration> cosmosDbConfigurationOptions)
    {
        _cosmosDbConfigurationOptions = cosmosDbConfigurationOptions;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_cosmosDbConfigurationOptions.Value);
    }
}
