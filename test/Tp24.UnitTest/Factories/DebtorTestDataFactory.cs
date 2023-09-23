using Tp24.Core.Domain.Entities;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.UnitTest.Factories;

public static class DebtorTestDataFactory
{
    public static DebtorDomainModel CreateDebtorDomainModel(
        string name = "Sample Debtor",
        string reference = "DEBT001",
        string address1 = "123 Debtor St",
        string address2 = "Apt 45B",
        string town = "Debtville",
        string state = "DS",
        string zip = "12345",
        string countryCode = "US",
        string registrationNumber = "REG12345"
    )
    {
        var debtor = new DebtorDomainModel(name, reference, countryCode)
        {
            Address1 = address1,
            Address2 = address2,
            Town = town,
            State = state,
            Zip = zip,
            CountryCode = countryCode,
            RegistrationNumber = registrationNumber
        };

        return debtor;
    }

    public static DebtorDataModel CreateDebtorDataModel(
        string name = "Sample Debtor",
        string reference = "DEBT001",
        string address1 = "123 Debtor St",
        string address2 = "Apt 45B",
        string town = "Debtville",
        string state = "DS",
        string zip = "12345",
        string countryCode = "US",
        string registrationNumber = "REG12345"
    )
    {
        return new DebtorDataModel
        {
            Name = name,
            Reference = reference,
            Address1 = address1,
            Address2 = address2,
            Town = town,
            State = state,
            Zip = zip,
            CountryCode = countryCode,
            RegistrationNumber = registrationNumber
        };
    }
}