# HouseHunter Agent Instructions

## Tech Stack

- C#
- .NET 9
- Console application
- Microsoft.Extensions.Hosting
- Dependency Injection
- Async/await preferred
- Visual Studio 2022

## Architecture Guidelines

- Keep services small and focused
- Use interfaces for services
- Avoid static classes unless necessary
- Prepare for future SQL Server persistence
- Use Microsoft.Data.SqlClient only
- Do not use Dapper

## Coding Style

- Prefer constructor injection (IServiceProvider as input parameter)
- Prefer async methods
- Keep Program.cs minimal