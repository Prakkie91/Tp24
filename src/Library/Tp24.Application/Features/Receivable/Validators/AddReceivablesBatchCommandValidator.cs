using FluentValidation;
using Tp24.Application.Features.Receivable.Commands;

namespace Tp24.Application.Features.Receivable.Validators;

public class AddReceivablesBatchCommandValidator : AbstractValidator<AddReceivablesBatchCommand>
{
    public AddReceivablesBatchCommandValidator()
    {
        RuleForEach(x => x.Receivables).SetValidator(new ReceivableDtoValidator());
    }
}