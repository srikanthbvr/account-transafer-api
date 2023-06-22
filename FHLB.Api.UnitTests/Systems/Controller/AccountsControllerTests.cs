using FHLB.Api.Controllers;
using FHLB.Api.Dtos;
using FHLB.Api.Entities;
using FHLB.Api.Repositories;
using FHLB.Api.UnitTests.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FHLB.Api.UnitTests;

public class AccountsControllerTests
{
    private readonly Mock<IAccountsRepository> AccountsRepositoryStub = new();
    private readonly Mock<ITransactionsRepository> TransactionsRepositoryStub = new();

    // private readonly Random rand = new();

    [Fact]
    // public void UnitOfWork_StateUnderTest_ExpectedBehavior()
    public async Task GetAccountAsync_WithUnexistingAccount_ReturnsNotFound()
    {
        // Arrange
        AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(It.IsAny<int>()))
            .ReturnsAsync((Account)null);

        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

        // Act
        var result = await controller.GetAccountAsync(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetAccountAsync_WithExistingAccount_ReturnsExpectedAccount()
    {
        // Arrange
        var expectedAccount = AccountsMockData.GetAccount();
        AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(It.IsAny<int>()))
            .ReturnsAsync(expectedAccount);

        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

        // Act
        var result = await controller.GetAccountAsync(1);

        // Assert
        Assert.IsType<AccountDto>(result.Value);
        var actualAccount = (AccountDto)result.Value;
        Assert.Equal(expectedAccount.Id, actualAccount.Id);
        Assert.Equal(expectedAccount.AccountName, actualAccount.AccountName);
        Assert.Equal(expectedAccount.AccountBalance, actualAccount.AccountBalance);
    }

    [Fact]
    public async Task GetAccountsAsync_WithExistingAccounts_ReturnsAllAccounts()
    {
        // Arrange
        var expectedAccounts = AccountsMockData.GetAccounts();
        AccountsRepositoryStub.Setup(repo => repo.GetAccountsAsync())
            .ReturnsAsync(expectedAccounts);

        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

        // Act
        var result = await controller.GetAccountsAsync();

        // Assert
        var actualAccounts = (IEnumerable<AccountDto>)result;
        Assert.Equal(expectedAccounts.Count(), actualAccounts.Count());
        for (int i = 0; i < expectedAccounts.Count(); i++)
        {
            Assert.Equal(expectedAccounts.ElementAt(i).Id, actualAccounts.ElementAt(i).Id);
            Assert.Equal(expectedAccounts.ElementAt(i).AccountName, actualAccounts.ElementAt(i).AccountName);
            Assert.Equal(expectedAccounts.ElementAt(i).AccountBalance, actualAccounts.ElementAt(i).AccountBalance);
        }
    }

    [Fact]
    public async Task CreateTransactionAsync_WithNonExistingAccount_ReturnsNotFound()
    {
        // Arrange
        CreateTransactionDto transactionDto = new()
        {
            FromAccountId = 10232,
            ToAccountId = 100,
            Amount = 10000
        };

        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

        // Act
        var result = await controller.CreateTransactionAsync(transactionDto);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateTransactionAsync_WithExistingAccount_ReturnsSuccessTransaction()
    {
        // Arrange
        var accounts = AccountsMockData.GetAccounts();
        var fromAccount = accounts.ElementAt(0);
        var toAccount = accounts.ElementAt(1);

        AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(fromAccount.Id))
            .ReturnsAsync(fromAccount);
        AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(toAccount.Id))
            .ReturnsAsync(toAccount);

        CreateTransactionDto transactionDto = new()
        {
            FromAccountId = fromAccount.Id,
            ToAccountId = toAccount.Id,
            Amount = 500
        };

        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

        // Act
        var result = await controller.CreateTransactionAsync(transactionDto);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);
        var actualTransaction = (result.Result as CreatedAtActionResult).Value as TransactionDto;
        Assert.Equal("Success", actualTransaction.Status);
    }

    [Fact]
    public async Task CreateTransactionAsync_WithExistingAccount_ReturnsBadRequest()
    {
        // Arrange
        var accounts = AccountsMockData.GetAccounts();
        var fromAccount = accounts.ElementAt(0);
        var toAccount = accounts.ElementAt(1);

        AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(fromAccount.Id))
            .ReturnsAsync(fromAccount);
        AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(toAccount.Id))
            .ReturnsAsync(toAccount);

        CreateTransactionDto transactionDto = new()
        {
            FromAccountId = fromAccount.Id,
            ToAccountId = toAccount.Id,
            Amount = 50000
        };

        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

        // Act
        var result = await controller.CreateTransactionAsync(transactionDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}