using System;
using Tp24.Core.Domain.Entities.Common;

namespace Tp24.Core.Domain.Entities
{
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

        public string Name { get; private set; }
        public string Reference { get; private set; }
        public string Address1 { get; private set; }
        public string Address2 { get; private set; }
        public string Town { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }
        public string CountryCode { get; private set; }
        public string RegistrationNumber { get; private set; }

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

        public void SetAddress1(string address1)
        {
            if (address1?.Length > 255)
                throw new ArgumentException("Invalid address1.", nameof(address1));

            Address1 = address1;
        }

        public void SetAddress2(string address2)
        {
            if (address2?.Length > 255)
                throw new ArgumentException("Invalid address2.", nameof(address2));

            Address2 = address2;
        }

        public void SetTown(string town)
        {
            if (town?.Length > 255)
                throw new ArgumentException("Invalid town.", nameof(town));

            Town = town;
        }

        public void SetState(string state)
        {
            if (state?.Length > 255)
                throw new ArgumentException("Invalid state.", nameof(state));

            State = state;
        }

        public void SetZip(string zip)
        {
            if (zip?.Length > 10)
                throw new ArgumentException("Invalid ZIP.", nameof(zip));

            Zip = zip;
        }

        public void SetRegistrationNumber(string registrationNumber)
        {
            if (registrationNumber?.Length > 255)
                throw new ArgumentException("Invalid registration number.", nameof(registrationNumber));

            RegistrationNumber = registrationNumber;
        }
    }
}
