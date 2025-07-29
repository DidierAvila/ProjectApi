using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Customers.Queries;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Customers.Handlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?> 
    {
        private readonly ICustomerRepositoy _customerRepositoy;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerRepositoy customerRepositoy, IMapper mapper)
        {
            _customerRepositoy = customerRepositoy;
            _mapper = mapper;
        }

        public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepositoy.GetByID(request.CustomerId, cancellationToken);
            return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
        }
    }
} 