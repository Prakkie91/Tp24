using AutoMapper;
using Tp24.Core.Domain.Entities;
using Tp24.Infrastructure.DataAccess.Entities;
using Tp24.UnitTest.Factories;
using Tp24.UnitTest.Fixtures;
using Xunit;

namespace Tp24.UnitTest.Mappings;

[Collection("AutoMapper Collection")]
public class ReceivableMappingTests
{
    private readonly IMapper _mapper;

    public ReceivableMappingTests(AutoMapperFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void ReceivableDataModel_To_ReceivableDomainModel_Mapping_IsValid()
    {
        // Arrange
        var receivableData = ReceivableTestDataFactory.CreateReceivableDataModel();

        // Act
        var receivableDomain = _mapper.Map<ReceivableDomainModel>(receivableData);

        // Assert
        Assert.NotNull(receivableDomain);
        Assert.Equal(receivableData.Reference, receivableDomain.Reference);
        Assert.Equal(receivableData.CurrencyCode, receivableDomain.CurrencyCode);
        Assert.Equal(receivableData.IssueDate, receivableDomain.IssueDate);
        Assert.Equal(receivableData.OpeningValue, receivableDomain.OpeningValue);
        Assert.Equal(receivableData.PaidValue, receivableDomain.PaidValue);
        Assert.Equal(receivableData.DueDate, receivableDomain.DueDate);
        Assert.Equal(receivableData.Cancelled, receivableDomain.Cancelled);
        Assert.Equal(receivableData.DebtorId, receivableDomain.DebtorId);
    }

    [Fact]
    public void ReceivableDomainModel_To_ReceivableDataModel_Mapping_IsValid()
    {
        // Arrange
        var receivableDomain = ReceivableTestDataFactory.CreateReceivableDomainModel();

        // Act
        var receivableData = _mapper.Map<ReceivableDataModel>(receivableDomain);

        // Assert
        Assert.NotNull(receivableData);
        Assert.Equal(receivableDomain.Reference, receivableData.Reference);
        Assert.Equal(receivableDomain.CurrencyCode, receivableData.CurrencyCode);
        Assert.Equal(receivableDomain.IssueDate, receivableData.IssueDate);
        Assert.Equal(receivableDomain.OpeningValue, receivableData.OpeningValue);
        Assert.Equal(receivableDomain.PaidValue, receivableData.PaidValue);
        Assert.Equal(receivableDomain.DueDate, receivableData.DueDate);
        Assert.Equal(receivableDomain.Cancelled, receivableData.Cancelled);
        Assert.Equal(receivableDomain.DebtorId, receivableData.DebtorId);
    }
}