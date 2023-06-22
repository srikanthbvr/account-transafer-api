using FHLB.Api.Entities;

namespace FHLB.Api.Repositories;

public interface IAccountsRepository
{
    IEnumerable<Account> GetAccounts();

    Account GetAccount(int id);

    void CreateAccount(Account account);

    void UpdateAccount(Account account);

    void DeleteAccount(int id);
}
