using System.Net.Mime;
using BookWorm.Catalog.Features.Books.Create;
using BookWorm.Constants;
using FluentValidation;

namespace BookWorm.Catalog.Features.Books.Update;

internal sealed class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
{
    private const int MaxFileSize = 1048576;

    public UpdateBookValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty().MaximumLength(DataSchemaLength.Medium);

        RuleFor(x => x.Description).MaximumLength(DataSchemaLength.SuperLarge);

        RuleFor(x => x.Price).GreaterThan(0);

        When(
            IsHasFiles,
            () =>
            {
                RuleFor(x => x.Image!)
                    .ChildRules(image =>
                    {
                        image
                            .RuleFor(x => x.Length)
                            .LessThanOrEqualTo(MaxFileSize)
                            .WithMessage(
                                $"The file size should not exceed {MaxFileSize / 1024} KB."
                            );
                        image
                            .RuleFor(x => x.ContentType)
                            .Must(x =>
                                x == MediaTypeNames.Image.Jpeg || x == MediaTypeNames.Image.Png
                            )
                            .WithMessage(
                                "File type is not allowed. Allowed file types are JPEG and PNG."
                            );
                    });
            }
        );
    }

    private bool IsHasFiles(UpdateBookCommand command)
    {
        return command.Image is not null;
    }
}
