using AutoMapper;
using Tp24.Core.Domain.Entities;
using Tp24.Infrastructure.DataAccess.Entities;
using Tp24.UnitTest.Factories;
using Tp24.UnitTest.Fixtures;
using Xunit;

namespace Tp24.UnitTest.Mappings;

[Collection("AutoMapper Collection")]
public class DebtorMappingTests
{
    private readonly IMapper _mapper;

    public DebtorMappingTests(AutoMapperFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void DebtorDataModel_To_DebtorDomainModel_Mapping_IsValid()
    {
        // Arrange
        var debtorData = DebtorTestDataFactory.CreateDebtorDataModel();

        // Act
        var debtorDomain = _mapper.Map<DebtorDomainModel>(debtorData);

        // Assert
        Assert.NotNull(debtorDomain);
        Assert.Equal(debtorData.Name, debtorDomain.Name);
        Assert.Equal(debtorData.Reference, debtorDomain.Reference);
        Assert.Equal(debtorData.Address1, debtorDomain.Address1);
        Assert.Equal(debtorData.Address2, debtorDomain.Address2);
        Assert.Equal(debtorData.Town, debtorDomain.Town);
        Assert.Equal(debtorData.State, debtorDomain.State);
        Assert.Equal(debtorData.Zip, debtorDomain.Zip);
        Assert.Equal(debtorData.CountryCode, debtorDomain.CountryCode);
        Assert.Equal(debtorData.RegistrationNumber, debtorDomain.RegistrationNumber);
    }

    [Fact]
    public void DebtorDomainModel_To_DebtorDataModel_Mapping_IsValid()
    {
        // Arrange
        var debtorDomain = DebtorTestDataFactory.CreateDebtorDomainModel();

        // Act
        var debtorData = _mapper.Map<DebtorDataModel>(debtorDomain);

        // Assert
        Assert.NotNull(debtorData);
        Assert.Equal(debtorDomain.Name, debtorData.Name);
        Assert.Equal(debtorDomain.Reference, debtorData.Reference);
        Assert.Equal(debtorDomain.Address1, debtorData.Address1);
        Assert.Equal(debtorDomain.Address2, debtorData.Address2);
        Assert.Equal(debtorDomain.Town, debtorData.Town);
        Assert.Equal(debtorDomain.State, debtorData.State);
        Assert.Equal(debtorDomain.Zip, debtorData.Zip);
        Assert.Equal(debtorDomain.CountryCode, debtorData.CountryCode);
        Assert.Equal(debtorDomain.RegistrationNumber, debtorData.RegistrationNumber);
    }
}
