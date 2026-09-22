using CollectionManagerApi.DTOs.Requests.CollectionItem;
using FluentValidation;

namespace CollectionManagerApi.Validators.CollectionItem;

public class CreateCollectionItemValidator : AbstractValidator<CreateCollectionItemRequest>
{
    public CreateCollectionItemValidator()
    {
        RuleFor(x => x.StorageId)
            .NotEmpty()
            .NotNull()
            .WithMessage("StorageId is required");
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");
        RuleFor(x => x.Comments)
            .MaximumLength(1000)
            .WithMessage("Comments must not exceed 1000 characters");
        RuleFor(x => x.HasDocuments)
            .NotNull()
            .WithMessage("HasDocuments must be set either true or false");
        RuleFor(x => x.IsDamaged)
            .NotNull()
            .WithMessage("IsDamaged must be set either true or false");
        RuleFor(x => x.IsOnDisplay)
            .NotNull()
            .WithMessage("IsDamaged must be set either true or false");
        
    }
}