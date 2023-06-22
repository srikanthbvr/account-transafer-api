# Code Challenge - Design and implement a REST API for transferring money between accounts

## Requirements

1. Coded in C# .NET
2. Keep it very simple and to the point (e.g. no need to implement any authentication).
3. You can use any frameworks/libraries you like but be sure to keep it simple.
4. The minimal datastore should run in-memory.
5. The final result should be executable as a standalone program (should not require a pre-installed container/server).
6. Demonstrate with tests that the API works as expected.

## Assumptions

1. The API will be used by a single client at a time.
2. The API will be used by a single thread at a time.

## Design

- C#
- ASP.NET Core
- SDK ??
- Unit tests - xUnit

### Commands

```cmd
dotnet new webapi -n FHLB.Api
dotnet new xunit -n FHLB.Api.UnitTests
dotnet add reference ../FHLB.Api/FHLB.Api.csproj
dotnet add package moq
dotnet add package FluentAssertions
dotnet test
```

### Entities

#### Account

- Id
- Name
- Balance

#### Transaction

- Id
- FromAccountId
- ToAccountId
- Amount
- Timestamp

## Development

## Unit Tests

## TODO

## Notes

