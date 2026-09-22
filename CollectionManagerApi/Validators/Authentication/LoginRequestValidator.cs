using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Authentication;
using FluentValidation;

namespace CollectionManagerApi.Validators.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is required");
        
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required");
    }
}