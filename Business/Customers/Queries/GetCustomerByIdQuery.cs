using Domain.Dtos;
using MediatR;

namespace Business.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto?>
    {
        public int CustomerId { get; set; }
    }
} 