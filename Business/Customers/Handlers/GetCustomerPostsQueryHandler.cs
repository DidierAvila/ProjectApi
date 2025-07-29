using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Customers.Queries;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Customers.Handlers
{
    public class GetCustomerPostsQueryHandler : IRequestHandler<GetCustomerPostsQuery, IEnumerable<PostDto>>
    {
        private readonly ICustomerRepositoy _customerRepositoy;
        private readonly IMapper _mapper;

        public GetCustomerPostsQueryHandler(ICustomerRepositoy customerRepositoy, IMapper mapper)
        {
            _customerRepositoy = customerRepositoy;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetCustomerPostsQuery request, CancellationToken cancellationToken)
        {
            PostDto posts = new(); //await _customerRepositoy.GetPostsByCustomerId(request.CustomerId, cancellationToken);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
} 