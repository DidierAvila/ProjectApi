using Domain.Entities;
using FluentValidation;

namespace Domain.Validator
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
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
                .MaximumLength(500).WithMessage("La categoría del post no puede exceder 500 caracteres")
                .Must(BeValidCategory).WithMessage("La categoría debe ser 'Farándula', 'Política', 'Futbol' o una categoría personalizada");

            RuleFor(x => x.Type)
                .InclusiveBetween(1, 10).WithMessage("El tipo debe estar entre 1 y 10");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("El ID del cliente debe ser mayor a 0");
        }

        private bool BeValidCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return false;

            var validCategories = new[] { "Farándula", "Política", "Futbol" };
            return validCategories.Contains(category) || category.Length <= 500;
        }
    }
} 