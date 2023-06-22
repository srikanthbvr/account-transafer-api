using System.Threading.Tasks;
using FHLB.Api.Entities;

namespace FHLB.Api.Repositories;

public interface IAccountsRepository
{
    Task<IEnumerable<Account>> GetAccountsAsync();

    Task<Account?> GetAccountAsync(int id);

    Task CreateAccountAsync(Account account);

    Task UpdateAccountAsync(Account account);

    Task DeleteAccountAsync(int id);
}
