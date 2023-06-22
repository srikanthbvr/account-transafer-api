using FHLB.Api.Entities;
using Microsoft.Extensions.ObjectPool;

namespace FHLB.Api.Repositories;

public class InMemTransactionsRepository : ITransactionsRepository
{
  public readonly List<Transaction> transactions = new();

  public Transaction CreateTransaction(Transaction transaction)
  {
    transactions.Add(transaction);
    return transaction;
  }
  public IEnumerable<Transaction> GetDebitTransactions(int id)
  {
    var debitTransactions = transactions.Where(transaction => transaction.FromAccountId == id);
    
    return debitTransactions;
  }

  public IEnumerable<Transaction> GetCreditTransactions(int id)
  {
    var creditTransactions = transactions.Where(transaction => transaction.ToAccountId == id);
    
    return creditTransactions;
  }

  public Transaction GetTransaction(int id)
  {
    return transactions.SingleOrDefault(transaction => transaction.Id == id);
  }

  public IEnumerable<Transaction> GetTransactions()
  {
    return transactions;
  }
}
