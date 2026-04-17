# Aspire + MCP + GitHub Models Demo

This repository contains the **final code** for a hands-on demo presented at **Global Azure**.

The demo shows how to:

- Build an **MCP server** with .NET
- Consume MCP tools from an **API**
- Use **.NET Aspire** to bootstrap and orchestrate everything
- Use **GitHub Models** so people can replicate the demo **without an Azure account**

> A step-by-step guide is available here:  
> https://sinannar.github.io/blog/007-globalazure2026/

## Solution Structure

- `AspireWithAI.AppHost`  
  Aspire orchestration project. Wires up MCP service, API service, and GitHub model resource.
- `AspireWithAI.Mcp`  
  MCP server exposing weather tool(s) over HTTP (`/mcp`).
- `AspireWithAI.Api`  
  API that connects to the MCP server, loads tools, and uses a chat model via Aspire integration.
- `AspireWithAI.ServiceDefaults`  
  Shared defaults (telemetry, service discovery, resilience).

## Architecture (High Level)

1. `AspireWithAI.Mcp` exposes MCP tool(s) (e.g., `DescribeWeather`).
2. `AspireWithAI.Api` connects to MCP, discovers tools, and uses them through an AI agent.
3. `AspireWithAI.AppHost` starts both projects and provides the `chat` model dependency using GitHub Models.

## Prerequisites

- .NET SDK 10
- Aspire workload/tooling supported by your SDK
- A GitHub account with access to GitHub Models
- A GitHub token with permissions to call GitHub Models (set as environment variable, e.g. `GITHUB_TOKEN`)

## Run Locally

From the repository root:

```bash
dotnet restore AspireWithAI.slnx
dotnet run --project ./AspireWithAI.AppHost/AspireWithAI.AppHost.csproj
```

Then:

1. Open the Aspire dashboard from the AppHost output.
2. Find the `api` service endpoint.
3. Call:

```http
GET /weatherforecast
```

The API response includes AI-generated weather descriptions produced through MCP tools.

## Notes

- The local demo path is designed to be reproducible without Azure by using GitHub Models.
- For the presentation scenario, deployment to Azure is also planned.

## Blog Post

For the full hands-on walkthrough, see:  
https://sinannar.github.io/blog/007-globalazure2026/
