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
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
    {
        private readonly ICustomerRepositoy _customerRepositoy;
        private readonly IMapper _mapper;

        public GetAllCustomersQueryHandler(ICustomerRepositoy customerRepositoy, IMapper mapper)
        {
            _customerRepositoy = customerRepositoy;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepositoy.GetAll(cancellationToken);
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }
    }
} 