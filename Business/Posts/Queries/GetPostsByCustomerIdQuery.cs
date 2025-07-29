using System.Collections.Generic;
using Domain.Dtos;
using MediatR;

namespace Business.Posts.Queries
{
    public class GetPostsByCustomerIdQuery : IRequest<IEnumerable<PostDto>>
    {
        public int CustomerId { get; set; }
    }
} 