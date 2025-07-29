using Business.Customers.Commands;
using FluentValidation;

namespace Business.Customers.Validators
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio")
                .MaximumLength(500).WithMessage("El nombre del cliente no puede exceder 500 caracteres")
                .MinimumLength(2).WithMessage("El nombre del cliente debe tener al menos 2 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$").WithMessage("El nombre del cliente solo puede contener letras y espacios");
        }
    }
} 