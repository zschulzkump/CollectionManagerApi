using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Account;
using FluentValidation;

namespace CollectionManagerApi.Validators.Account;

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is required");
        
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .Must(x => x.Any(char.IsUpper))
            .Must(x => x.Any(char.IsLower))
            .Must(x => x.Any(char.IsDigit))
            .WithMessage("Password is required");
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty()
            .WithMessage("First name is required");
        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Last name is required");
        
        
    }
}