using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Authentication;
using FluentValidation;

namespace CollectionManagerApi.Validators.Authentication;

public class LoginRefreshTokenRequestValdiator : AbstractValidator<LoginRefreshTokenRequest>
{
    public LoginRefreshTokenRequestValdiator()
    {
        RuleFor(x => x.RefreshToken)
            .NotNull()
            .NotEmpty()
            .WithMessage("RefreshToken is required");
    }
}