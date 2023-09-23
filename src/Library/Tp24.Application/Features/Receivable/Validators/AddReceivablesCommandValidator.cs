using FluentValidation;
using Tp24.Application.Features.Receivable.Commands;

namespace Tp24.Application.Features.Receivable.Validators;

public class AddReceivablesCommandValidator : AbstractValidator<AddReceivablesCommand>
{
    public AddReceivablesCommandValidator()
    {
        Include(new ReceivableDtoValidator()); // Include validation rules from ReceivableDtoValidator
    }
}