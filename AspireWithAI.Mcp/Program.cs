using System.ComponentModel;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.Stateless = true;
    })
    .WithToolsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.MapMcp("/mcp");

app.Run();

[McpServerToolType]
public static class WeatherDescriberMcpTools
{
    [McpServerTool, Description("Describes the weather for given degree in celcius")]
    public static string DescribeWeather(
        [Description("degree in celcius")] int degree) => degree switch
        {
            < 0 => "Freezing",
            < 10 => "Cold",
            < 20 => "Mild",
            < 30 => "Warm",
            _ => "Hot"
        };
}