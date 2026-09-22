using CollectionManagerApi.DTOs.Requests.CollectionGroup;
using FluentValidation;

namespace CollectionManagerApi.Validators.CollectionGroup;

public class UpdateCollectionGroupRequestValidator : AbstractValidator<UpdateCollectionGroupRequest>
{
    public UpdateCollectionGroupRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Collection Group Name is required");
    }
}