using CollectionManagerApi.DTOs.Requests.CollectionItemAttribute;
using FluentValidation;

namespace CollectionManagerApi.Validators.CollectionItemAttribute;

public class UpdateCollectionItemAttributeValidator : AbstractValidator<UpdateCollectionItemAttributeRequest>
{
    public UpdateCollectionItemAttributeValidator()
    {
        RuleFor(request => request.CustomAttributeId)
            .NotEmpty()
            .NotNull()
            .WithMessage("The custom attribute id cannot be empty");
        RuleFor(request => request.CollectionItemId)
            .NotEmpty()
            .NotNull()
            .WithMessage("The collectionItemId cannot be empty");
        RuleFor(request => request.DataType)
            .NotEmpty()
            .NotNull()
            .WithMessage("The data type cannot be empty");
        RuleFor(request => request.Value)
            .NotEmpty()
            .NotNull()
            .WithMessage("The value cannot be empty");
    }
}