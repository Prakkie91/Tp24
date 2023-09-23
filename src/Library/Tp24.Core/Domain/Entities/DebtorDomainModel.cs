using Tp24.Core.Domain.Entities.Common;

namespace Tp24.Core.Domain.Entities;

public class DebtorDomainModel : BaseDomainModel
{
    public DebtorDomainModel()
    {
    }

    public DebtorDomainModel(string name, string reference, string countryCode)
    {
        SetName(name);
        SetReference(reference);
        SetCountryCode(countryCode);
    }

    public string Name { get; set; }
    public string Reference { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Town { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string CountryCode { get; set; }
    public string RegistrationNumber { get; set; }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 255)
            throw new ArgumentException("Invalid name.", nameof(name));

        Name = name;
    }

    private void SetReference(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference) || reference.Length > 255)
            throw new ArgumentException("Invalid reference.", nameof(reference));

        Reference = reference;
    }

    private void SetCountryCode(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            throw new ArgumentException("Invalid country code.", nameof(countryCode));

        CountryCode = countryCode;
    }
}