# Code Challenge - Design and implement a REST API for transferring money between accounts

## Requirements

1. Coded in C# .NET
2. Keep it very simple and to the point (e.g. no need to implement any authentication).
3. You can use any frameworks/libraries you like but be sure to keep it simple.
4. The minimal datastore should run in-memory.
5. The final result should be executable as a standalone program (should not require a pre-installed container/server).
6. Demonstrate with tests that the API works as expected.

## Sope

1. Develop APIs
2. Develop Unit Tests

## Design

- C#
- ASP.NET Core
- SDK 7.0
- Unit tests - xUnit



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

Use Visual Studio Code to develop the API.
Develop two controllers handling actions for accounts and transactions.

## Unit Tests

Create Unit test cases covering all the scenarios.
Uses
- Fluent Assertions
- Moq
- xUnit
## Notes

### Commands

```cmd
dotnet new webapi -n FHLB.Api
dotnet new xunit -n FHLB.Api.UnitTests
dotnet add reference ../FHLB.Api/FHLB.Api.csproj
dotnet add package moq
dotnet add package FluentAssertions
dotnet test
```
