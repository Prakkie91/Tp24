using FluentValidation;
using Tp24.Application.Features.Receivable.Commands;

namespace Tp24.Application.Features.Receivable.Validators;

public class ReceivableDtoValidator : AbstractValidator<ReceivableDto>
{
    public ReceivableDtoValidator()
    {
        RuleFor(x => x.Reference)
            .NotEmpty()
            .MaximumLength(50).WithMessage("Reference should not exceed 50 characters.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .Length(3).WithMessage("Currency code should be exactly 3 characters long.");

        RuleFor(x => x.IssueDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Issue date should not be in the future.");

        RuleFor(x => x.OpeningValue)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Opening value should be positive.");

        RuleFor(x => x.PaidValue)
            .NotEmpty()
            .GreaterThanOrEqualTo(0).WithMessage("Paid value cannot be negative.")
            .LessThanOrEqualTo(x => x.OpeningValue).WithMessage("Paid value should not exceed opening value.");

        RuleFor(x => x.DueDate)
            .NotEmpty();

        RuleFor(x => x.ClosedDate)
            .LessThanOrEqualTo(DateTime.Now).When(x => x.ClosedDate.HasValue)
            .WithMessage("Closed date should not be in the future.");

        RuleFor(x => x.DebtorName)
            .NotEmpty()
            .MaximumLength(255).WithMessage("Debtor name should not exceed 255 characters.");

        RuleFor(x => x.DebtorReference)
            .NotEmpty()
            .MaximumLength(255).WithMessage("Debtor reference should not exceed 255 characters.");

        RuleFor(x => x.DebtorCountryCode)
            .NotEmpty()
            .Length(2).WithMessage("Country code should be exactly 2 characters long.");

        RuleFor(x => x.DebtorRegistrationNumber)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.DebtorRegistrationNumber))
            .WithMessage("Debtor registration number should not exceed 255 characters.");

        RuleFor(x => x.DebtorZip)
            .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.DebtorZip))
            .WithMessage("Debtor ZIP should not exceed 20 characters.");
    }
}