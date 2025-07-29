using Domain.Dtos;
using MediatR;

namespace Business.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string Name { get; set; } = string.Empty;
    }
} 