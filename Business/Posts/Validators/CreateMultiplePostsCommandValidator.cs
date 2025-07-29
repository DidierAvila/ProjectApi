using Business.Posts.Commands;
using FluentValidation;

namespace Business.Posts.Validators
{
    public class CreateMultiplePostsCommandValidator : AbstractValidator<CreateMultiplePostsCommand>
    {
        public CreateMultiplePostsCommandValidator()
        {
            RuleFor(x => x.Posts)
                .NotEmpty().WithMessage("La lista de posts no puede estar vacía")
                .Must(posts => posts != null && posts.Count > 0 && posts.Count <= 100)
                .WithMessage("Debe proporcionar entre 1 y 100 posts");

            RuleForEach(x => x.Posts).SetValidator(new CreatePostRequestValidator());
        }
    }

    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título del post es obligatorio")
                .MaximumLength(500).WithMessage("El título del post no puede exceder 500 caracteres")
                .MinimumLength(3).WithMessage("El título del post debe tener al menos 3 caracteres");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("El contenido del post es obligatorio")
                .MaximumLength(500).WithMessage("El contenido del post no puede exceder 500 caracteres")
                .MinimumLength(10).WithMessage("El contenido del post debe tener al menos 10 caracteres");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("La categoría del post es obligatoria")
                .MaximumLength(500).WithMessage("La categoría del post no puede exceder 500 caracteres");

            RuleFor(x => x.Type)
                .InclusiveBetween(1, 10).WithMessage("El tipo debe estar entre 1 y 10");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor a 0");
        }
    }
} 