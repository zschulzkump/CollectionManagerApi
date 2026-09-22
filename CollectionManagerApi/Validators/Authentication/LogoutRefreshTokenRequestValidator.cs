using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Authentication;
using FluentValidation;

namespace CollectionManagerApi.Validators.Authentication;

public class LogoutRefreshTokenRequestValidator : AbstractValidator<LogoutRefreshTokenRequest>
{
    public LogoutRefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotNull()
            .NotEmpty()
            .WithMessage("RefreshToken is required");
    }
}