using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Posts.Queries;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Posts.Handlers
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto?>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostByIdQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<PostDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByID(request.PostId, cancellationToken);
            return post != null ? _mapper.Map<PostDto>(post) : null;
        }
    }
} 