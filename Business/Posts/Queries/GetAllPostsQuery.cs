using System.Collections.Generic;
using Domain.Dtos;
using MediatR;

namespace Business.Posts.Queries
{
    public class GetAllPostsQuery : IRequest<IEnumerable<PostDto>>
    {
    }
} 