using System.Threading.Tasks;
using FHLB.Api.Dtos;
using FHLB.Api.Entities;
using FHLB.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FHLB.Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TransactionsController : ControllerBase
  {
    private readonly IAccountsRepository accountsRepository;
    private readonly ITransactionsRepository transactionsRepository;

    public TransactionsController(IAccountsRepository accountsRepository, ITransactionsRepository transactionsRepository)
    {
      this.accountsRepository = accountsRepository;
      this.transactionsRepository = transactionsRepository;
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetTransactionAsync))]
    public async Task<ActionResult<TransactionDto>> GetTransactionAsync(int id)
    {
      var transaction = await transactionsRepository.GetTransactionAsync(id);

      if (transaction is null)
      {
        return NotFound();
      }

      return transaction.AsDto();
    }

    [HttpGet]
    public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync()
    {
      var transactions = (await transactionsRepository.GetTransactionsAsync())
                          .Select(transaction => transaction.AsDto());

      return transactions;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransactionAsync(CreateTransactionDto transactionDto)
    {
      var fromAccount = await accountsRepository.GetAccountAsync(transactionDto.FromAccountId);
      var toAccount = await accountsRepository.GetAccountAsync(transactionDto.ToAccountId);

      if (fromAccount is null || toAccount is null)
      {
        return NotFound();
      }

      int transactionFee = fromAccount.AccountType == "Savings" ? 5 : 0;

      bool insufficientFunds = fromAccount.AccountBalance < (transactionDto.Amount + transactionFee);

      Transaction transaction = new()
      {
        Id = transactionsRepository.GetTransactionsAsync().Result.Count() + 1,
        FromAccountId = transactionDto.FromAccountId,
        ToAccountId = transactionDto.ToAccountId,
        Amount = transactionDto.Amount,
        Date = DateTime.UtcNow,
        Status = insufficientFunds ? "Failed" : "Success",
        ErrorMessage = insufficientFunds ? "Insufficient funds" : null,
      };

      await transactionsRepository.CreateTransactionAsync(transaction);

      if (insufficientFunds)
      {
        return BadRequest("Insufficient funds");
      }

      fromAccount.Withdraw(transactionDto.Amount);
      toAccount.Deposit(transactionDto.Amount);

      if (transactionFee > 0)
      {
        Transaction feeTransaction = new()
        {
          Id = transactionsRepository.GetTransactionsAsync().Result.Count() + 1,
          FromAccountId = transactionDto.FromAccountId,
          ToAccountId = 999999,
          Amount = transactionFee,
          Date = DateTime.UtcNow,
          Status = insufficientFunds ? "Failed" : "Success",
          ErrorMessage = insufficientFunds ? "Insufficient funds" : null,
        };

        await transactionsRepository.CreateTransactionAsync(feeTransaction);
        fromAccount.Withdraw(transactionFee);
      }

      await accountsRepository.UpdateAccountAsync(fromAccount);
      await accountsRepository.UpdateAccountAsync(toAccount);

      return CreatedAtAction(nameof(GetTransactionAsync), new { id = transaction.Id }, transaction.AsDto());
    }
  }
}