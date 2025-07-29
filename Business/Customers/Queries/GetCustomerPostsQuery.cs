using System.Collections.Generic;
using Domain.Dtos;
using MediatR;

namespace Business.Customers.Queries
{
    public class GetCustomerPostsQuery : IRequest<IEnumerable<PostDto>>
    {
        public int CustomerId { get; set; }
    }
} 