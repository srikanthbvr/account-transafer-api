using FHLB.Api.Entities;

namespace FHLB.Api.Repositories;

public class InMemAccountsRepository : IAccountsRepository
{

  public readonly List<Account> accounts = new()
  {
    new Account { Id = 1, AccountName = "Robin", AccountBalance = 1000 },
    new Account { Id = 2, AccountName = "Micheal", AccountBalance = 2000 },
    new Account { Id = 3, AccountName = "Mary", AccountBalance = 3000 },
  };

  public Account GetAccount(int id)
  {
    return accounts.Find(existingItem => existingItem.Id == id);
  }

  public IEnumerable<Account> GetAccounts()
  {
    return accounts;
  }

  public void CreateAccount(Account account)
  {
    accounts.Add(account);
  }

  public void UpdateAccount(Account account)
  {
    var index = accounts.FindIndex(existingItem => existingItem.Id == account.Id);
    accounts[index] = account;
  }

  public void DeleteAccount(int id)
  {
    var index = accounts.FindIndex(existingItem => existingItem.Id == id);
    accounts.RemoveAt(index);
  }
}
