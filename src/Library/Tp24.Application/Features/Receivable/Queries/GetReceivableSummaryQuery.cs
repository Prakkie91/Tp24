using MediatR;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Common.Wrappers;

namespace Tp24.Application.Features.Receivable.Queries;

public class GetReceivableSummaryQuery : IRequest<IResult<ReceivableSummaryResponse>>
{
}