using CollectionManagerApi.DTOs.Requests.CollectionGroup;
using FluentValidation;

namespace CollectionManagerApi.Validators.CollectionGroup;

public class CreateCollectionGroupRequestValidator : AbstractValidator<CreateCollectionGroupRequest>
{
    public CreateCollectionGroupRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Collection Group Name is required");
    }
}