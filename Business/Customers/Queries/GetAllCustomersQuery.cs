using System.Collections.Generic;
using Domain.Dtos;
using MediatR;

namespace Business.Customers.Queries
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>
    {
    }
} 