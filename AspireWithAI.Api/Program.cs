using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.AddServiceDefaults();
builder.AddAzureChatCompletionsClient("chat")
    .AddChatClient();

builder.Services.AddActivatedSingleton(sp =>
{
    var mcpClient = McpClient.CreateAsync(new HttpClientTransport(
    new HttpClientTransportOptions()
    {
        Endpoint = new (Environment.GetEnvironmentVariable("services__mcp__https__0")! + "/mcp")
    })).GetAwaiter().GetResult();
    return mcpClient;
});

builder.Services.AddActivatedKeyedSingleton("weatheragent", (sp, _) =>
{
    var mcpClient = sp.GetRequiredService<McpClient>();
    var tools = mcpClient.ListToolsAsync().GetAwaiter().GetResult();
    var chatClient = sp.CreateAsyncScope().ServiceProvider.GetRequiredService<IChatClient>();
    return chatClient.AsAIAgent(
        name: "weatheragent",
        instructions: "you take the weather related information as JSON and provide a summary, including description of the degree in celcius using the mcp tools",
        tools: [.. tools.Cast<AITool>()]
    );
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

app.MapGet("/weatherforecast", ([FromKeyedServices("weatheragent")] ChatClientAgent weatheragent) =>
{
    var forecast =  Enumerable.Range(1, 2).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            weatheragent.RunAsync($"What is the weather like for a temperature of {Random.Shared.Next(-20, 55)} degree in celcius? Include 3 different language, but include English and Turkish for sure. Response will be shown to human, so no json or other structured format.").GetAwaiter().GetResult()!.Text
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
app.MapDefaultEndpoints();

app.Run();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}