using System.Threading.Tasks;
namespace FHLB.Api.Entities;

public interface ITransactionsRepository
{
    Task<Transaction> CreateTransactionAsync(Transaction transaction);

    Task<IEnumerable<Transaction>> GetDebitTransactions(int id);

    Task<IEnumerable<Transaction>> GetCreditTransactions(int id);

    Task<Transaction?> GetTransactionAsync(int id);
    Task<IEnumerable<Transaction>> GetTransactionsAsync();
}
