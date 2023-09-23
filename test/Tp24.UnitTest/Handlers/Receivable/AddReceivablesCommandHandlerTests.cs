using Microsoft.Extensions.Logging;
using Moq;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Application.Features.Receivable.Handlers;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;
using Tp24.UnitTest.Factories;
using Xunit;

namespace Tp24.UnitTest.Handlers.Receivable;

[Collection("Handler Collection")]
public class AddReceivablesCommandHandlerTests
{
    private readonly Mock<IDebtorRepository> _debtorRepositoryMock;
    private readonly AddReceivablesCommandHandler _handler;
    private readonly Mock<IReceivableRepository> _receivableRepositoryMock;

    public AddReceivablesCommandHandlerTests()
    {
        _receivableRepositoryMock = new Mock<IReceivableRepository>();
        _debtorRepositoryMock = new Mock<IDebtorRepository>();
        Mock<ILogger<AddReceivablesCommandHandler>> receivableLoggerMock = new();

        _handler = new AddReceivablesCommandHandler(
            _receivableRepositoryMock.Object,
            _debtorRepositoryMock.Object,
            receivableLoggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenDebtorExistsButReceivableDoesNot_CreatesNewReceivable()
    {
        // Arrange
        var command = ReceivableTestDataFactory.CreateAddReceivablesCommand();
        var existingDebtor = DebtorTestDataFactory.CreateDebtorDomainModel();
        _debtorRepositoryMock.Setup(r => r.FindByReferenceAsync(It.IsAny<string>())).ReturnsAsync(existingDebtor);
        _receivableRepositoryMock.Setup(r => r.FindByReferenceAsync(It.IsAny<string>()))
            .ReturnsAsync((ReceivableDomainModel)null);
        
        _receivableRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ReceivableDomainModel>()))
            .ReturnsAsync(ReceivableTestDataFactory.CreateReceivableDomainModel());
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal("Receivable added successfully.", result.Messages.FirstOrDefault());
        _receivableRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ReceivableDomainModel>()), Times.Once());
    }

    [Fact]
    public async Task Handle_WhenNeitherDebtorNorReceivableExist_CreatesBoth()
    {
        // Arrange
        var command = ReceivableTestDataFactory.CreateAddReceivablesCommand();
        _debtorRepositoryMock.Setup(r => r.FindByReferenceAsync(It.IsAny<string>()))
            .ReturnsAsync((DebtorDomainModel)null);
        _receivableRepositoryMock.Setup(r => r.FindByReferenceAsync(It.IsAny<string>()))
            .ReturnsAsync((ReceivableDomainModel)null);
        
        _receivableRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ReceivableDomainModel>()))
            .ReturnsAsync(ReceivableTestDataFactory.CreateReceivableDomainModel());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal("Receivable added successfully.", result.Messages.FirstOrDefault());
        _debtorRepositoryMock.Verify(r => r.AddAsync(It.IsAny<DebtorDomainModel>()), Times.Once());
        _receivableRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ReceivableDomainModel>()), Times.Once());
    }

    [Fact]
    public async Task Handle_WhenReceivableAlreadyExists_DoesNotCreateReceivable()
    {
        // Arrange
        var command = ReceivableTestDataFactory.CreateAddReceivablesCommand();
        var existingDebtor = DebtorTestDataFactory.CreateDebtorDomainModel();
        var existingReceivable = ReceivableTestDataFactory.CreateReceivableDomainModel();
        _debtorRepositoryMock.Setup(r => r.FindByReferenceAsync(It.IsAny<string>())).ReturnsAsync(existingDebtor);
        _receivableRepositoryMock.Setup(r => r.FindByReferenceAsync(It.IsAny<string>()))
            .ReturnsAsync(existingReceivable);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Receivable with the same reference already exists. No changes were made.",
            result.Messages.FirstOrDefault());
        _receivableRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ReceivableDomainModel>()), Times.Never());
    }

    [Fact]
    public void Handle_WithInvalidData_ThrowsArgumentException()
    {
        // Arrange
        var command = new AddReceivablesCommand(); // Missing crucial data

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult());
    }
}