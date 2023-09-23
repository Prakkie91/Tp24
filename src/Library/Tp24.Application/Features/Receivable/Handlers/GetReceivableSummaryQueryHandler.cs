using MediatR;
using Tp24.Application.Features.Receivable.Queries;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Common.Wrappers;
using Tp24.Core.Interfaces.Repositories;

namespace Tp24.Application.Features.Receivable.Handlers;

/// <summary>
///     Handler to retrieve a summary of receivables.
/// </summary>
public class
    GetReceivableSummaryQueryHandler : IRequestHandler<GetReceivableSummaryQuery, IResult<ReceivableSummaryResponse>>
{
    private readonly IReceivableRepository _receivableRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetReceivableSummaryQueryHandler" /> class.
    /// </summary>
    /// <param name="receivableRepository">The repository to fetch receivable summaries.</param>
    public GetReceivableSummaryQueryHandler(IReceivableRepository receivableRepository)
    {
        _receivableRepository = receivableRepository ?? throw new ArgumentNullException(nameof(receivableRepository));
    }

    /// <summary>
    ///     Handles the query to fetch a summary of receivables.
    /// </summary>
    /// <param name="request">The request/query to retrieve the summary.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The summary of receivables wrapped in a result object.</returns>
    public async Task<IResult<ReceivableSummaryResponse>> Handle(GetReceivableSummaryQuery request,
        CancellationToken cancellationToken)
    {
        // Fetch the summary of receivables from the repository.
        var summary = await _receivableRepository.GetReceivablesSummaryAsync();

        // Transform the domain model summary into an application layer response model.
        var response = new ReceivableSummaryResponse
        {
            TotalReceivables = summary.TotalReceivables,
            OpenInvoiceCount = summary.OpenInvoiceCount,
            ClosedInvoiceCount = summary.ClosedInvoiceCount,
            TotalOpeningValue = summary.TotalOpeningValue,
            TotalPaidValue = summary.TotalPaidValue,
            NumOfOverdueInvoices = summary.NumOfOverdueInvoices,
            TotalOverdueAmount = summary.TotalOverdueAmount,
            UniqueDebtors = summary.UniqueDebtors
        };

        // Return the mapped response wrapped in a result object.
        return await Result<ReceivableSummaryResponse>.SuccessAsync(response);
    }
}