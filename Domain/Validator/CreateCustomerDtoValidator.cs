using Domain.Dtos;
using FluentValidation;

namespace Domain.Validator
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio")
                .MaximumLength(500).WithMessage("El nombre del cliente no puede exceder 500 caracteres")
                .MinimumLength(2).WithMessage("El nombre del cliente debe tener al menos 2 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre del cliente solo puede contener letras y espacios");
        }
    }
} 