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

      return CreatedAtAction(nameof(GetAccountAsync), new { id = account.Id });
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

    [HttpGet]
    [Route("{id}/transactions")]
    public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(int id)
    {
      var transactions = (await transactionsRepository.GetTransactionsAsync())
                          .Where(transaction => transaction.FromAccountId == id || transaction.ToAccountId == id)
                          .Select(transaction => transaction.AsDto());

      return transactions;
    }

    [HttpGet]
    [Route("transactions/{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransactionAsync(int id)
    {
      var transaction = await transactionsRepository.GetTransactionAsync(id);

      if (transaction is null)
      {
        return NotFound();
      }

      return transaction.AsDto();
    }

    [HttpPost]
    [Route("transactions")]
    public async Task<ActionResult<TransactionDto>> CreateTransactionAsync(CreateTransactionDto transactionDto)
    {
      var fromAccount = await accountsRepository.GetAccountAsync(transactionDto.FromAccountId);
      var toAccount = await accountsRepository.GetAccountAsync(transactionDto.ToAccountId);

      if (fromAccount is null || toAccount is null)
      {
        return NotFound();
      }

      bool insufficientFunds = fromAccount.AccountBalance < transactionDto.Amount;

      Transaction transaction = new()
      {
        Id = transactionsRepository.GetTransactionsAsync().Result.Count() + 1,
        FromAccountId = transactionDto.FromAccountId,
        ToAccountId = transactionDto.ToAccountId,
        Amount = transactionDto.Amount,
        Date = DateTime.UtcNow,
        Status = insufficientFunds ? "Failed" : "Success"
      };

      await transactionsRepository.CreateTransactionAsync(transaction);

      if (insufficientFunds)
      {
        return BadRequest("Insufficient funds");
      }

      fromAccount.Withdraw(transactionDto.Amount);
      toAccount.Deposit(transactionDto.Amount);

      await accountsRepository.UpdateAccountAsync(fromAccount);
      await accountsRepository.UpdateAccountAsync(toAccount);

      return CreatedAtAction(nameof(GetTransactionAsync), new { id = transaction.Id }, transaction.AsDto());
    }
  }
}