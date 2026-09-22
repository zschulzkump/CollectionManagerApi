using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using FluentValidation;

namespace CollectionManagerApi.Validators.CollectionItemImage;

public class UpdateCollectionItemImagesValidator : AbstractValidator<UpdateCollectionItemImagesRequest>
{
    public UpdateCollectionItemImagesValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(80)
            .WithMessage("Name is required");
        RuleFor(x =>x.IsThumbnail)
            .NotNull()
            .WithMessage("IsThumbnail must be set either true or false");
    }
}