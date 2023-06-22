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
    public ActionResult<IEnumerable<AccountDto>> GetAccounts()
    {
      var accounts = accountsRepository.GetAccounts().Select(account => account.AsDto());

      return Ok(accounts);
    }

    [HttpGet("{id}")]
    public ActionResult<AccountDto?> GetAccount(int id)
    {
      var account = accountsRepository.GetAccount(id);

      if (account is null)
      {
        return NotFound();
      }

      return Ok(account.AsDto());
    }

    [HttpPost]
    public ActionResult<AccountDto> CreateAccount(CreateAccountDto accountDto)
    {
      Account account = new()
      {
        Id = accountsRepository.GetAccounts().Count() + 1,
        AccountName = accountDto.AccountName,
        AccountBalance = 0
      };

      accountsRepository.CreateAccount(account);

      return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account.AsDto());
    }

    [HttpPut("{id}")]
    public ActionResult UpdateAccount(int id, CreateAccountDto accountDto)
    {
      var existingAccount = accountsRepository.GetAccount(id);

      if (existingAccount is null)
      {
        return NotFound();
      }

      Account updatedAccount = existingAccount with
      {
        AccountName = accountDto.AccountName
      };

      accountsRepository.UpdateAccount(updatedAccount);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteAccount(int id)
    {
      var existingAccount = accountsRepository.GetAccount(id);

      if (existingAccount is null)
      {
        return NotFound();
      }

      accountsRepository.DeleteAccount(id);

      return NoContent();
    }

    [HttpGet]
    [Route("{id}/transactions")]
    public ActionResult<IEnumerable<TransactionDto>> GetTransactions(int id)
    {
      var account = accountsRepository.GetAccount(id);

      if (account is null)
      {
        return NotFound();
      }

      IEnumerable<TransactionDto> debitTransactions = transactionsRepository.GetDebitTransactions(id).Select(transaction => transaction.AsDto());
      debitTransactions = debitTransactions.Select(transaction => transaction with { TransactionType = "Debit" });

      IEnumerable<TransactionDto> creditTransactions = transactionsRepository.GetCreditTransactions(id).Select(transaction => transaction.AsDto());
      creditTransactions = creditTransactions.Select(transaction => transaction with { TransactionType = "Credit" });

      return Ok(debitTransactions.Concat(creditTransactions));
    }

    [HttpGet]
    [Route("transactions/{id}")]
    public ActionResult<TransactionDto?> GetTransaction(int id)
    {
      var transaction = transactionsRepository.GetTransaction(id);

      if (transaction is null)
      {
        return NotFound();
      }

      return Ok(transaction.AsDto());
    }

    [HttpPost]
    [Route("transactions")]
    public ActionResult<TransactionDto> CreateTransaction(CreateTransactionDto transactionDto)
    {
      var fromAccount = accountsRepository.GetAccount(transactionDto.FromAccountId);
      var toAccount = accountsRepository.GetAccount(transactionDto.ToAccountId);

      if (fromAccount is null || toAccount is null)
      {
        return NotFound();
      }

      bool insufficientFunds = fromAccount.AccountBalance < transactionDto.Amount;

      Transaction transaction = new()
      {
        Id = transactionsRepository.GetTransactions().Count() + 1,
        FromAccountId = transactionDto.FromAccountId,
        ToAccountId = transactionDto.ToAccountId,
        Amount = transactionDto.Amount,
        Date = DateTime.UtcNow,
        Status = (insufficientFunds ? "Failed" : "Success")
      };

      transactionsRepository.CreateTransaction(transaction);

      if (insufficientFunds)
      {
        return BadRequest(new { message = "Insufficient funds" });
      }

      fromAccount.Withdraw(transactionDto.Amount);
      toAccount.Deposit(transactionDto.Amount);

      accountsRepository.UpdateAccount(fromAccount);
      accountsRepository.UpdateAccount(toAccount);

      return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction.AsDto());
    }

  }
}