using CollectionManagerApi.DTOs.Requests.Tags;
using FluentValidation;

namespace CollectionManagerApi.Validators.Tag;

public class UpdateTagRequestValidator : AbstractValidator<UpdateTagRequest>
{
    public UpdateTagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");
    }
}