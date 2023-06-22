using System.Collections.Generic;
using System.Threading.Tasks;
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

  public async Task<IEnumerable<Account>> GetAccountsAsync()
  {
    return await Task.FromResult(accounts);
  }

  public async Task<Account?> GetAccountAsync(int id)
  {
    var account = accounts.Where(_ => _.Id == id).SingleOrDefault();
    return await Task.FromResult(account);
  }

  public async Task CreateAccountAsync(Account account)
  {
    accounts.Add(account);
    await Task.CompletedTask;
  }

  public async Task UpdateAccountAsync(Account account)
  {
    var index = accounts.FindIndex(_ => _.Id == account.Id);
    accounts[index] = account;
    await Task.CompletedTask;
  }

  public async Task DeleteAccountAsync(int id)
  {
    var index = accounts.FindIndex(existingItem => existingItem.Id == id);
    accounts.RemoveAt(index);
    await Task.CompletedTask;
  }
}
