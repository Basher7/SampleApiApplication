using Application.Commands.Auth;
using FluentValidation;

namespace Application.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginCommand>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.userName)
            .NotNull().NotEmpty().WithMessage("Username is required.")
            .MinimumLength(5).WithMessage("Username must be at least 5 characters long.")
            .MaximumLength(15).WithMessage("Username must not exceed 15 characters.")
            .Matches(@"^(?=^[A-Za-z])(?=[A-Za-z0-9._]{3,15}$)(?!.*[_.]{2})[A-Za-z0-9._]+(?<![_.])$")
            .WithMessage("Username can only contain letters, numbers, underscores, or dots, must start with a letter, and cannot end with a dot or underscore.");

        RuleFor(x => x.password)
            .NotNull().NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(20).WithMessage("Password must not exceed 20 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=!])(?!.*\s).{8,20}$")
            .WithMessage("Password must contain upper and lowercase letters, a digit, a special characters(@#$%^&+=!), and no spaces.");
    }
}
