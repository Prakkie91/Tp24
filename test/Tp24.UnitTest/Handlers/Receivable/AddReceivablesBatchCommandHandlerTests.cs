using Moq;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Application.Features.Receivable.Handlers;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;
using Tp24.UnitTest.Factories;
using Xunit;

namespace Tp24.UnitTest.Handlers.Receivable;

[Collection("Handler Collection")]
public class AddReceivablesBatchCommandHandlerTests
{
    private readonly Mock<IDebtorRepository> _debtorRepositoryMock;
    private readonly AddReceivablesBatchCommandHandler _handler;
    private readonly Mock<IReceivableRepository> _receivableRepositoryMock;

    public AddReceivablesBatchCommandHandlerTests()
    {
        _receivableRepositoryMock = new Mock<IReceivableRepository>();
        _debtorRepositoryMock = new Mock<IDebtorRepository>();

        _handler = new AddReceivablesBatchCommandHandler(
            _receivableRepositoryMock.Object,
            _debtorRepositoryMock.Object
        );

        SetupDebtorReturnMocks();
        SetupReceivableReturnMocks();
    }

    private void SetupDebtorReturnMocks()
    {
        var passedDebtors = new List<DebtorDomainModel>();
        _debtorRepositoryMock.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<DebtorDomainModel>>()))
            .Callback<IEnumerable<DebtorDomainModel>>(d => passedDebtors = d.ToList())
            .ReturnsAsync(() => passedDebtors);

        var passedDebtor = new DebtorDomainModel();
        _debtorRepositoryMock.Setup(r => r.AddAsync(It.IsAny<DebtorDomainModel>()))
            .Callback<DebtorDomainModel>(d => passedDebtor = d)
            .ReturnsAsync(() => passedDebtor);
    }

    private void SetupReceivableReturnMocks()
    {
        // Setup for AddRangeAsync
        var passedReceivables = new List<ReceivableDomainModel>();
        _receivableRepositoryMock.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<ReceivableDomainModel>>()))
            .Callback<IEnumerable<ReceivableDomainModel>>(r => passedReceivables = r.ToList())
            .ReturnsAsync(() => passedReceivables);

        // Setup for AddAsync
        var passedReceivable = new ReceivableDomainModel();
        _receivableRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ReceivableDomainModel>()))
            .Callback<ReceivableDomainModel>(r => passedReceivable = r)
            .ReturnsAsync(() => passedReceivable);
    }


    [Fact]
    public async Task Handle_ShouldAddNewDebtorsAndReceivables()
    {
        // Arrange
        var command = ReceivableTestDataFactory.CreateAddReceivablesBatchCommand();
        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>());
        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        _debtorRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<DebtorDomainModel>>()), Times.Once());
        _receivableRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<ReceivableDomainModel>>()),
            Times.Once());

        Assert.NotNull(result.Data);
        Assert.Equal(0, result.Data.ExistingCount);
        Assert.Equal(0, result.Data.DuplicatesCount);
        Assert.Equal(command.Receivables.Count, result.Data.SuccessCount);
        Assert.Equal(command.Receivables.Count, result.Data.TotalProcessed);
    }

    [Fact]
    public async Task Handle_WithDuplicateDebtorReferences_ExtractsUniqueDebtors()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "123"),
            ReceivableTestDataFactory.CreateReceivableDto("2", debtorReference: "123"),
            ReceivableTestDataFactory.CreateReceivableDto("3", debtorReference: "456")
        };

        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>()); // return no existing debtors

        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>()); // return no existing receivables

        // Act
        var result = await _handler.Handle(new AddReceivablesBatchCommand { Receivables = receivables },
            CancellationToken.None);

        // Assert
        _debtorRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<DebtorDomainModel>>()), Times.Once);

        var invocation = _debtorRepositoryMock.Invocations
            .FirstOrDefault(inv => inv.Method.Name == "AddRangeAsync");

        var addedDebtors = invocation?.Arguments[0] as IEnumerable<DebtorDomainModel>;

        Assert.Equal(2, addedDebtors.Count()); // Should have added 2 unique debtors

        Assert.NotNull(result.Data);
        Assert.Equal(0, result.Data.ExistingCount);
        Assert.Equal(0, result.Data.DuplicatesCount);
        Assert.Equal(receivables.Count, result.Data.SuccessCount);
        Assert.Equal(receivables.Count, result.Data.TotalProcessed);
    }

    [Fact]
    public async Task Handle_WithNewDebtors_AddsThemSuccessfully()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "789"),
            ReceivableTestDataFactory.CreateReceivableDto("2", debtorReference: "101")
        };

        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>()); // return no existing debtors

        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>()); // return no existing receivables

        // Act
        var result = await _handler.Handle(new AddReceivablesBatchCommand { Receivables = receivables },
            CancellationToken.None);

        // Assert
        _debtorRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<DebtorDomainModel>>()), Times.Once);

        Assert.NotNull(result.Data);
        Assert.Equal(0, result.Data.ExistingCount);
        Assert.Equal(0, result.Data.DuplicatesCount);
        Assert.Equal(receivables.Count, result.Data.SuccessCount);
        Assert.Equal(receivables.Count, result.Data.TotalProcessed);
    }

    [Fact]
    public async Task Handle_WithMixOfExistingAndNewDebtors_AddsOnlyNewDebtors()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "789"),
            ReceivableTestDataFactory.CreateReceivableDto("2", debtorReference: "101"),
            ReceivableTestDataFactory.CreateReceivableDto("3", debtorReference: "123")
        };

        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>
            {
                DebtorTestDataFactory.CreateDebtorDomainModel(reference: "123")
            });

        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>()); // return no existing receivables

        // Act
        var result = await _handler.Handle(new AddReceivablesBatchCommand { Receivables = receivables },
            CancellationToken.None);

        // Assert
        _debtorRepositoryMock.Verify(
            r => r.AddRangeAsync(It.Is<IEnumerable<DebtorDomainModel>>(list => list.Count() == 2)),
            Times.Once);

        Assert.NotNull(result.Data);
        Assert.Equal(0, result.Data.ExistingCount);
        Assert.Equal(0, result.Data.DuplicatesCount);
        Assert.Equal(receivables.Count, result.Data.SuccessCount);
        Assert.Equal(receivables.Count, result.Data.TotalProcessed);
    }

    [Fact]
    public async Task Handle_WithOnlyExistingDebtors_DoesNotAddAny()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "123"),
            ReceivableTestDataFactory.CreateReceivableDto("2", debtorReference: "456")
        };

        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>
            {
                DebtorTestDataFactory.CreateDebtorDomainModel(reference: "123"),
                DebtorTestDataFactory.CreateDebtorDomainModel(reference: "456")
            });

        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>()); // return no existing receivables

        // Act
        var result = await _handler.Handle(new AddReceivablesBatchCommand { Receivables = receivables },
            CancellationToken.None);

        // Assert
        _debtorRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<List<DebtorDomainModel>>()), Times.Never);

        Assert.NotNull(result.Data);
        Assert.Equal(0, result.Data.ExistingCount);
        Assert.Equal(0, result.Data.DuplicatesCount);
        Assert.Equal(receivables.Count, result.Data.SuccessCount);
        Assert.Equal(receivables.Count, result.Data.TotalProcessed);
    }

    [Fact]
    public async Task Handle_WithSomeReceivablesAlreadyInSystem_PreventsReInsertion()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "123"),
            ReceivableTestDataFactory.CreateReceivableDto("2", debtorReference: "456")
        };

        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>
            {
                ReceivableTestDataFactory.CreateReceivableDomainModel("1")
            });

        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>());

        // Act
        var result = await _handler.Handle(new AddReceivablesBatchCommand { Receivables = receivables },
            CancellationToken.None);

        // Assert
        _receivableRepositoryMock.Verify(
            r => r.AddRangeAsync(It.Is<List<ReceivableDomainModel>>(list => list.Count == 1)), Times.Once);


        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.ExistingCount);
        Assert.Equal(0, result.Data.DuplicatesCount);
        Assert.Equal(1, result.Data.SuccessCount);
        Assert.Equal(2, result.Data.TotalProcessed);
    }

    [Fact]
    public async Task Handle_WithDuplicateReferencesInSameBatch_FiltersOutDuplicates()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "123"),
            ReceivableTestDataFactory.CreateReceivableDto("2", debtorReference: "456"),
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "123")
        };

        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>());

        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>());

        // Act
        var result = await _handler.Handle(new AddReceivablesBatchCommand { Receivables = receivables },
            CancellationToken.None);

        // Assert
        _receivableRepositoryMock.Verify(
            r => r.AddRangeAsync(It.Is<List<ReceivableDomainModel>>(list => list.Count == 2)), Times.Once);


        Assert.NotNull(result.Data);
        Assert.Equal(0, result.Data.ExistingCount);
        Assert.Equal(1, result.Data.DuplicatesCount);
        Assert.Equal(2, result.Data.SuccessCount);
        Assert.Equal(3, result.Data.TotalProcessed);
    }

    [Fact]
    public async Task Handle_WithValidData_SuccessfullyInsertsReceivables()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto("1", debtorReference: "123"),
            ReceivableTestDataFactory.CreateReceivableDto("2", debtorReference: "456")
        };

        _debtorRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<DebtorDomainModel>());

        _receivableRepositoryMock.Setup(r => r.FindByReferencesAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new List<ReceivableDomainModel>());

        // Act
        var result = await _handler.Handle(new AddReceivablesBatchCommand { Receivables = receivables },
            CancellationToken.None);

        // Assert
        _receivableRepositoryMock.Verify(
            r => r.AddRangeAsync(It.Is<List<ReceivableDomainModel>>(list => list.Count == 2)), Times.Once);

        Assert.NotNull(result.Data);
        Assert.Equal(0, result.Data.ExistingCount);
        Assert.Equal(0, result.Data.DuplicatesCount);
        Assert.Equal(2, result.Data.SuccessCount);
        Assert.Equal(2, result.Data.TotalProcessed);
    }
}