using MediatR;
using Tp24.Common.Wrappers;

namespace Tp24.Application.Features.Receivable.Commands;

public class AddReceivablesCommand : ReceivableDto, IRequest<IResult<Guid>>
{
}