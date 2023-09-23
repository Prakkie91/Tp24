using MediatR;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Common.Wrappers;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;

namespace Tp24.Application.Features.Receivable.Handlers;

/// <summary>
///     Handles the addition of a batch of receivables.
///     This handler ensures that new debtors are added to the system and then processes and inserts the receivables.
/// </summary>
public class
    AddReceivablesBatchCommandHandler : IRequestHandler<AddReceivablesBatchCommand,
        IResult<AddReceivablesBatchResponse>>
{
    private readonly IDebtorRepository _debtorRepository;
    private readonly IReceivableRepository _receivableRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AddReceivablesBatchCommandHandler" /> class.
    /// </summary>
    /// <param name="receivableRepository">Repository for handling receivable data operations.</param>
    /// <param name="debtorRepository">Repository for handling debtor data operations.</param>
    public AddReceivablesBatchCommandHandler(IReceivableRepository receivableRepository,
        IDebtorRepository debtorRepository)
    {
        _receivableRepository = receivableRepository ?? throw new ArgumentNullException(nameof(receivableRepository));
        _debtorRepository = debtorRepository ?? throw new ArgumentNullException(nameof(debtorRepository));
    }

    /// <summary>
    ///     Handles the addition of a batch of receivables.
    /// </summary>
    /// <param name="request">Request containing receivables data.</param>
    /// <param name="cancellationToken">Token for task cancellation.</param>
    /// <returns>Result indicating the outcome of the batch processing.</returns>
    public async Task<IResult<AddReceivablesBatchResponse>> Handle(AddReceivablesBatchCommand request,
        CancellationToken cancellationToken)
    {
        var response = new AddReceivablesBatchResponse();

        // Extract unique debtors from the request.
        var uniqueDebtors = GetUniqueDebtors(request.Receivables);

        // Find existing debtors in the system.
        var existingDebtors =
            await _debtorRepository.FindByReferencesAsync(uniqueDebtors.Select(d => d.DebtorReference).ToList());

        // Determine which debtors need to be added.
        var newDebtorsToAdd = GetDebtorsToBeAdded(uniqueDebtors, existingDebtors);

        // Add new debtors to the system.
        var addedDebtors = await _debtorRepository.AddRangeAsync(MapToDebtorDomainModels(newDebtorsToAdd));

        // Get the list of already existing receivable references
        var existingReceivables =
            await _receivableRepository.FindByReferencesAsync(request.Receivables.Select(r => r.Reference));

        // Prepare receivables for insertion.
        var preparationResult = PrepareReceivablesForInsertion(request.Receivables, addedDebtors, existingDebtors,
            existingReceivables.Select(a => a.Reference));

        await _receivableRepository.AddRangeAsync(preparationResult.ReceivablesToInsert);

        response.SuccessCount = preparationResult.ReceivablesToInsert.Count;
        response.DuplicatesCount = preparationResult.DuplicatesCount;
        response.ExistingCount = preparationResult.ExistingCount;
        response.TotalProcessed = request.Receivables.Count;

        return await Result<AddReceivablesBatchResponse>.SuccessAsync(response, "Batch processed successfully.");
    }


    /// <summary>
    ///     Extracts unique debtors from the list of receivables.
    /// </summary>
    /// <param name="receivables">List of receivables.</param>
    /// <returns>List of unique debtors.</returns>
    private List<ReceivableDto> GetUniqueDebtors(IEnumerable<ReceivableDto> receivables)
    {
        return receivables.GroupBy(r => r.DebtorReference).Select(g => g.First()).ToList();
    }

    /// <summary>
    ///     Determines which debtors from the list are not already in the system.
    /// </summary>
    /// <param name="uniqueDebtors">List of unique debtors.</param>
    /// <param name="existingDebtors">List of debtors that already exist in the system.</param>
    /// <returns>List of debtors that need to be added.</returns>
    private IEnumerable<ReceivableDto> GetDebtorsToBeAdded(IEnumerable<ReceivableDto> uniqueDebtors,
        IEnumerable<DebtorDomainModel> existingDebtors)
    {
        var existingReferences = existingDebtors.Select(a => a.Reference);
        return uniqueDebtors.Where(d => !existingReferences.Contains(d.DebtorReference));
    }

    /// <summary>
    ///     Maps debtor DTOs to domain models.
    /// </summary>
    /// <param name="debtors">List of debtor DTOs.</param>
    /// <returns>List of debtor domain models.</returns>
    private IEnumerable<DebtorDomainModel> MapToDebtorDomainModels(IEnumerable<ReceivableDto> debtors)
    {
        return debtors.Select(d => new DebtorDomainModel(d.DebtorName, d.DebtorReference, d.DebtorCountryCode)
        {
            Address1 = d.DebtorAddress1,
            Address2 = d.DebtorAddress2,
            Town = d.DebtorTown,
            State = d.DebtorState,
            Zip = d.DebtorZip,
            RegistrationNumber = d.DebtorRegistrationNumber
        });
    }

    /// <summary>
    ///     Prepares receivables for insertion by associating them with the appropriate debtor.
    /// </summary>
    /// <param name="receivableDtos">List of receivable DTOs.</param>
    /// <param name="newlyAdded">List of newly added debtors.</param>
    /// <param name="existingDebtors">List of debtors that already exist in the system.</param>
    /// <param name="existingReceivableReferences"></param>
    /// <returns>List of receivables ready for insertion.</returns>
    private (List<ReceivableDomainModel> ReceivablesToInsert, int DuplicatesCount, int ExistingCount)
        PrepareReceivablesForInsertion(
            List<ReceivableDto> receivableDtos,
            List<DebtorDomainModel> newlyAdded,
            List<DebtorDomainModel> existingDebtors,
            IEnumerable<string> existingReceivableReferences)
    {
        var receivables = new List<ReceivableDomainModel>();
        var duplicatesCount = receivableDtos.Count - receivableDtos.Select(r => r.Reference).Distinct().Count();
        var existingCount = receivableDtos.Count(r => existingReceivableReferences.Contains(r.Reference));

        // Filter out duplicates and existing receivables
        var uniqueReceivables = receivableDtos
            .Where(r => !existingReceivableReferences.Contains(r.Reference))
            .GroupBy(r => r.Reference)
            .Select(g => g.First()).ToList();

        foreach (var dto in uniqueReceivables)
        {
            var debtor = newlyAdded.FirstOrDefault(d => d.Reference == dto.DebtorReference)
                         ?? existingDebtors.FirstOrDefault(d => d.Reference == dto.DebtorReference);

            var receivable = new ReceivableDomainModel(
                dto.Reference,
                dto.CurrencyCode,
                dto.IssueDate,
                dto.OpeningValue,
                dto.PaidValue,
                dto.DueDate,
                debtor);

            receivables.Add(receivable);
        }

        return (receivables, duplicatesCount, existingCount);
    }
}