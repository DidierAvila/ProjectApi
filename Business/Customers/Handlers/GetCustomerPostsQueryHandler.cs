using AutoMapper;
using Business.Customers.Queries;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Customers.Handlers
{
    public class GetCustomerPostsQueryHandler : IRequestHandler<GetCustomerPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetCustomerPostsQueryHandler(IMapper mapper, IPostRepository postRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetCustomerPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetPostsByCustomerId(request.CustomerId, cancellationToken);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
} 