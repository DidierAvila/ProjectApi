using Business.Posts.Commands;
using FluentValidation;

namespace Business.Posts.Validators
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
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