using CollectionManagerApi.DTOs.Requests.Storages;
using FluentValidation;

namespace CollectionManagerApi.Validators.Storage;

public class StorageRequestValidator : AbstractValidator<StorageRequest>
{
    public StorageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Storage Name is required.");;
    }
}