var builder = DistributedApplication.CreateBuilder(args);

var mcp = builder.AddProject<Projects.AspireWithAI_Mcp>("mcp");

builder.AddProject<Projects.AspireWithAI_Api>("api")
    .WithReference(mcp).WaitFor(mcp);

builder.Build().Run();
