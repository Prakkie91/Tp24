using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Tp24.Infrastructure.DataAccess.Entities.Common;

namespace Tp24.Infrastructure.DataAccess.Entities;

/// <summary>
///     Represents a debtor entity, containing detailed information about a specific debtor.
/// </summary>
[Table("Debtor")]
public class DebtorDataModel : BaseEntity
{
    /// <summary>
    ///     Gets or sets the name of the debtor.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the reference number or code for the debtor.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Reference { get; set; }

    /// <summary>
    ///     Gets or sets the first line of the address for the debtor.
    /// </summary>
    [MaxLength(255)]
    [AllowNull]
    public string Address1 { get; set; }

    /// <summary>
    ///     Gets or sets the second line of the address for the debtor.
    /// </summary>
    [MaxLength(255)]
    [AllowNull]
    public string Address2 { get; set; }

    /// <summary>
    ///     Gets or sets the town or city name of the debtor's address.
    /// </summary>
    [MaxLength(255)]
    [AllowNull]
    public string Town { get; set; }

    /// <summary>
    ///     Gets or sets the state or province of the debtor's address.
    /// </summary>
    [MaxLength(255)]
    [AllowNull]
    public string State { get; set; }

    /// <summary>
    ///     Gets or sets the ZIP or postal code of the debtor's address.
    /// </summary>
    [MaxLength(10)]
    [AllowNull]
    public string Zip { get; set; }

    /// <summary>
    ///     Gets or sets the country code representing the debtor's country of residence.
    /// </summary>
    [Required]
    [MaxLength(3)]
    public string CountryCode { get; set; }

    /// <summary>
    ///     Gets or sets the registration number, if available, for the debtor.
    /// </summary>
    [MaxLength(255)]
    [AllowNull]
    public string RegistrationNumber { get; set; }
}