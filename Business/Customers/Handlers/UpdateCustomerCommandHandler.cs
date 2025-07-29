using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Customers.Commands;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Customers.Handlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepositoy _customerRepositoy;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ICustomerRepositoy customerRepositoy, IMapper mapper)
        {
            _customerRepositoy = customerRepositoy;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new InvalidOperationException("El nombre del cliente no puede estar vacío");
            }

            // Buscar el customer existente
            var customer = await _customerRepositoy.GetByID(request.CustomerId, cancellationToken);
            if (customer == null)
            {
                throw new InvalidOperationException($"No se encontró el cliente con ID {request.CustomerId}");
            }

            // Validar que no exista otro customer con el mismo nombre (excluyendo el actual)
            var existingCustomer = await _customerRepositoy.Find(c => c.Name.ToLower() == request.Name.ToLower() && c.CustomerId != request.CustomerId, cancellationToken);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException($"Ya existe otro cliente con el nombre '{request.Name}'");
            }

            // Actualizar el customer
            customer.Name = request.Name.Trim();
            await _customerRepositoy.Update(customer, cancellationToken);

            // Mapear la entidad actualizada a DTO usando AutoMapper
            return _mapper.Map<CustomerDto>(customer);
        }
    }
} 