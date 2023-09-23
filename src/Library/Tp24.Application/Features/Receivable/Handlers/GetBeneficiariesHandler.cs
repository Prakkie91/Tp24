using MediatR;
using Tp24.Application.Features.Receivable.Queries;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Core.Interfaces.Repositories;
using Tp24.Common.Wrappers;

namespace Tp24.Application.Features.Receivable.Handlers
{
    public class
        GetReceivableSummaryQueryHandler : IRequestHandler<GetReceivableSummaryQuery,
            IResult<ReceivableSummaryResponse>>
    {
        private readonly IReceivableRepository _receivableRepository;

        public GetReceivableSummaryQueryHandler(IReceivableRepository receivableRepository)
        {
            _receivableRepository =
                receivableRepository ?? throw new ArgumentNullException(nameof(receivableRepository));
        }

        public async Task<IResult<ReceivableSummaryResponse>> Handle(GetReceivableSummaryQuery request,
            CancellationToken cancellationToken)
        {
            // Fetch the summary from the repository
            var summary = await _receivableRepository.GetReceivablesSummaryAsync();

            // Map the summary domain model to the response model
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

            return await Result<ReceivableSummaryResponse>.SuccessAsync(response);
        }
    }
}