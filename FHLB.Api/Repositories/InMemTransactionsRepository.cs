using System.Runtime.CompilerServices;
using FHLB.Api.Entities;
using Microsoft.Extensions.ObjectPool;

namespace FHLB.Api.Repositories;

public class InMemTransactionsRepository : ITransactionsRepository
{
  public readonly List<Transaction> transactions = new();

  public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
  {
    transactions.Add(transaction);
    return await Task.FromResult(transaction);
  }
  public async Task<IEnumerable<Transaction>> GetDebitTransactions(int id)
  {
    var debitTransactions = transactions.Where(transaction => transaction.FromAccountId == id);
    return await Task.FromResult(debitTransactions);
  }

  public async Task<IEnumerable<Transaction>> GetCreditTransactions(int id)
  {
    var creditTransactions = transactions.Where(transaction => transaction.ToAccountId == id);
    return await Task.FromResult(creditTransactions);
  }

  public async Task<Transaction?> GetTransactionAsync(int id)
  {
    var transaction = transactions.Where(_ => _.Id == id).SingleOrDefault();
    return await Task.FromResult(transaction);
  }

  public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
  {
    return await Task.FromResult(transactions);
  }
}
