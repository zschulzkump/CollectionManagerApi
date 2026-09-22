using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using FluentValidation;

namespace CollectionManagerApi.Validators.CollectionItemImage;

public class CreateCollectionItemImagesValidator : AbstractValidator<CreateCollectionItemImagesRequest>
{
    public CreateCollectionItemImagesValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(80)
            .WithMessage("Name is required");
        RuleFor(x =>x.IsThumbnail)
            .NotNull()
            .WithMessage("IsThumbnail must be set either true or false");
        // RuleFor(x => x.File)
        //     .NotNull()
        //     .NotEmpty()
        //     .WithMessage("Image File is required");
        RuleFor(x => x.FileName)
            .NotNull()
            .NotEmpty()
            .WithMessage("FileName is required");
        RuleFor(x => x.ContentType)
            .NotNull()
            .NotEmpty()
            .WithMessage("ContentType is required");
    }
}