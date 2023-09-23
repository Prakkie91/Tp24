using MediatR;
using Microsoft.Extensions.Logging;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Common.Wrappers;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;

namespace Tp24.Application.Features.Receivable.Handlers;

/// <summary>
///     Handler for adding receivables.
/// </summary>
public class AddReceivablesCommandHandler : IRequestHandler<AddReceivablesCommand, IResult<Guid>>
{
    private readonly IDebtorRepository _debtorRepository;
    private readonly ILogger<AddReceivablesCommandHandler> _logger;
    private readonly IReceivableRepository _receivableRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AddReceivablesCommandHandler" /> class.
    /// </summary>
    /// <param name="receivableRepository">The repository for receivables.</param>
    /// <param name="debtorRepository">The repository for debtors.</param>
    /// <param name="logger">The logger instance for logging.</param>
    public AddReceivablesCommandHandler(
        IReceivableRepository receivableRepository,
        IDebtorRepository debtorRepository,
        ILogger<AddReceivablesCommandHandler> logger)
    {
        _receivableRepository = receivableRepository ?? throw new ArgumentNullException(nameof(receivableRepository));
        _debtorRepository = debtorRepository ?? throw new ArgumentNullException(nameof(debtorRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Handles the addition of a new receivable.
    /// </summary>
    /// <param name="request">The request to add a new receivable.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A result indicating success or failure of the operation.</returns>
    public async Task<IResult<Guid>> Handle(AddReceivablesCommand request, CancellationToken cancellationToken)
    {
        // Determine if a debtor exists, or create a new one.
        var debtor = await GetOrCreateDebtorAsync(request);

        // Check if a receivable with the same reference already exists.
        var existingReceivable = await _receivableRepository.FindByReferenceAsync(request.Reference);
        if (existingReceivable != null)
        {
            _logger.LogWarning("Receivable with reference {RequestReference} already exists", request.Reference);
            return await Result<Guid>.FailAsync(
                "Receivable with the same reference already exists. No changes were made.");
        }

        // Create and persist a new receivable.
        var receivable = new ReceivableDomainModel(
            request.Reference,
            request.CurrencyCode,
            request.IssueDate,
            request.OpeningValue,
            request.PaidValue,
            request.DueDate,
            debtor);

        var result = await _receivableRepository.AddAsync(receivable);
        _logger.LogInformation("Receivable with reference {RequestReference} added successfully", request.Reference);

        return await Result<Guid>.SuccessAsync(result.Id, "Receivable added successfully.");
    }

    /// <summary>
    ///     Gets an existing debtor by reference or creates a new one if it doesn't exist.
    /// </summary>
    /// <param name="request">The request to find or create a debtor.</param>
    /// <returns>The debtor, either retrieved or newly created.</returns>
    private async Task<DebtorDomainModel> GetOrCreateDebtorAsync(AddReceivablesCommand request)
    {
        // Check for an existing debtor with the provided reference.
        var existingDebtor = await _debtorRepository.FindByReferenceAsync(request.DebtorReference);

        if (existingDebtor != null) return existingDebtor;

        // Create a new debtor and persist it.
        var debtor = new DebtorDomainModel(
            request.DebtorName,
            request.DebtorReference,
            request.DebtorCountryCode)
        {
            Address1 = request.DebtorAddress1,
            Address2 = request.DebtorAddress2,
            Town = request.DebtorTown,
            State = request.DebtorState,
            Zip = request.DebtorZip,
            RegistrationNumber = request.DebtorRegistrationNumber
        };

        await _debtorRepository.AddAsync(debtor);
        _logger.LogInformation("New debtor with reference {RequestDebtorReference} created", request.DebtorReference);

        return debtor;
    }
}