using Domain.Dtos;
using MediatR;

namespace Business.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<CustomerDto>
    {
        public int CustomerId { get; set; }
    }
} 