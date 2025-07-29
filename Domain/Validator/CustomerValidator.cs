using Domain.Entities;
using FluentValidation;

namespace Domain.Validator
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio")
                .MaximumLength(500).WithMessage("El nombre del cliente no puede exceder 500 caracteres")
                .MinimumLength(2).WithMessage("El nombre del cliente debe tener al menos 2 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre del cliente solo puede contener letras y espacios");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).When(x => x.CustomerId > 0).WithMessage("El ID del cliente debe ser mayor a 0");
        }
    }
} 