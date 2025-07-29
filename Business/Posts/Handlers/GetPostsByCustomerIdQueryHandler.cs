using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Posts.Queries;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Posts.Handlers
{
    public class GetPostsByCustomerIdQueryHandler : IRequestHandler<GetPostsByCustomerIdQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostsByCustomerIdQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetPostsByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.Find(p => p.CustomerId == request.CustomerId, cancellationToken);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
} 