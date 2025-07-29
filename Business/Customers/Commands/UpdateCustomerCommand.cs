using Domain.Dtos;
using MediatR;

namespace Business.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<CustomerDto>
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
} 