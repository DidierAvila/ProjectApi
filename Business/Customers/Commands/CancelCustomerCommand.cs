using Domain.Dtos;
using MediatR;

namespace Business.Customers.Commands
{
    public class CancelCustomerCommand : IRequest<CustomerDto>
    {
        public int CustomerId { get; set; }
    }
}