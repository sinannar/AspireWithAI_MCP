using Aspire.Hosting.GitHub;

var builder = DistributedApplication.CreateBuilder(args);

var mcp = builder.AddProject<Projects.AspireWithAI_Mcp>("mcp");

var chat = builder.AddGitHubModel("chat", GitHubModel.OpenAI.OpenAIGpt4oMini);

builder.AddProject<Projects.AspireWithAI_Api>("api")
    .WithReference(mcp).WaitFor(mcp)
    .WithReference(chat).WaitFor(chat);

builder.Build().Run();
