using FHLB.Api.Dtos;
using FHLB.Api.Entities;

namespace FHLB.Api.UnitTests.MockData;
public static class AccountsMockData
{
  public static IEnumerable<Account> GetAccounts()
  {
    var accounts = new List<Account>
    {
      new()
      {
        Id = 1,
        AccountName = "Account 1",
        AccountBalance = 1000
      },
      new()
      {
        Id = 2,
        AccountName = "Account 2",
        AccountBalance = 2000
      },
      new()
      {
        Id = 3,
        AccountName = "Account 3",
        AccountBalance = 3000
      }
    };
    return accounts;
  }

  public static Account GetAccount()
  {
    return new()
    {
      Id = 1,
      AccountName = "Account 1",
      AccountBalance = 1000
    };
  }

  public static AccountDto GetAccountDto()
  {
    return new()
    {
      Id = 1,
      AccountName = "Account 1",
      AccountBalance = 1000
    };
  }

  public static Account GetAccountToCreate()
  {
    return new()
    {
      AccountName = "Account " + new Random().Next(1, 1000),
      AccountBalance = 1000
    };
  }
}