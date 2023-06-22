using System.Threading.Tasks;
using FHLB.Api.Dtos;
using FHLB.Api.Entities;
using FHLB.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FHLB.Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccountsController : ControllerBase
  {
    private readonly IAccountsRepository accountsRepository;
    private readonly ITransactionsRepository transactionsRepository;

    public AccountsController(IAccountsRepository accountsRepository, ITransactionsRepository transactionsRepository)
    {
      this.accountsRepository = accountsRepository;
      this.transactionsRepository = transactionsRepository;
    }

    [HttpGet]
    //GetAccountsAsync
    public async Task<IEnumerable<AccountDto>> GetAccountsAsync()
    {
      var accounts = (await accountsRepository.GetAccountsAsync())
                      .Select(account => account.AsDto());

      return accounts;
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetAccountAsync))]
    public async Task<ActionResult<AccountDto>> GetAccountAsync(int id)
    {
      var account = await accountsRepository.GetAccountAsync(id);

      if (account is null)
      {
        return NotFound();
      }

      return account.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccountAsync(CreateAccountDto accountDto)
    {
      Account account = new()
      {
        Id = accountsRepository.GetAccountsAsync().Result.Count() + 1,
        AccountName = accountDto.AccountName,
        AccountBalance = 0
      };

      await accountsRepository.CreateAccountAsync(account);

      return CreatedAtAction(nameof(GetAccountAsync), new { id = account.Id }, account.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAccountAsync(int id, CreateAccountDto accountDto)
    {
      var existingAccount = await accountsRepository.GetAccountAsync(id);

      if (existingAccount is null)
      {
        return NotFound();
      }

      Account updatedAccount = existingAccount with
      {
        AccountName = accountDto.AccountName
      };

      await accountsRepository.UpdateAccountAsync(updatedAccount);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAccountAsync(int id)
    {
      var existingAccount = await accountsRepository.GetAccountAsync(id);

      if (existingAccount is null)
      {
        return NotFound();
      }

      await accountsRepository.DeleteAccountAsync(id);

      return NoContent();
    }
  }
}