using MediatR;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Common.Wrappers;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;

namespace Tp24.Application.Features.Receivable.Handlers;

public class AddReceivablesCommandHandler : IRequestHandler<AddReceivablesCommand, IResult>
{
    private readonly IDebtorRepository _debtorRepository;
    private readonly IReceivableRepository _receivableRepository;

    public AddReceivablesCommandHandler(IReceivableRepository receivableRepository, IDebtorRepository debtorRepository)
    {
        _receivableRepository = receivableRepository;
        _debtorRepository = debtorRepository;
    }

    public async Task<IResult> Handle(AddReceivablesCommand request, CancellationToken cancellationToken)
    {
        // Checking if the debtor exists
        var existingDebtor = await _debtorRepository.FindByReferenceAsync(request.DebtorReference);

        DebtorDomainModel debtor;
        if (existingDebtor == null)
        {
            debtor = new DebtorDomainModel(
                request.DebtorName,
                request.DebtorReference,
                request.DebtorCountryCode);

            debtor.SetAddress1(request.DebtorAddress1);
            debtor.SetAddress2(request.DebtorAddress2);
            debtor.SetTown(request.DebtorTown);
            debtor.SetState(request.DebtorState);
            debtor.SetZip(request.DebtorZip);
            debtor.SetRegistrationNumber(request.DebtorRegistrationNumber);

            await _debtorRepository.AddAsync(debtor);
        }
        else
        {
            debtor = existingDebtor;
        }

        // Check if the receivable with the same reference already exists to ensure idempotency
        var existingReceivable = await _receivableRepository.FindByReferenceAsync(request.Reference);

        if (existingReceivable == null)
        {
            var receivable = new ReceivableDomainModel(
                request.Reference,
                request.CurrencyCode,
                request.IssueDate,
                request.OpeningValue,
                request.PaidValue,
                request.DueDate,
                debtor);

            // Save the receivable
            await _receivableRepository.AddAsync(receivable);
            return await Result.SuccessAsync("Receivable added successfully.");
        }
        else
        {
            // For idempotency, if the receivable already exists, don't re-add it.
            return await Result.FailAsync("Receivable with the same reference already exists. No changes were made.");
        }
    }
}
