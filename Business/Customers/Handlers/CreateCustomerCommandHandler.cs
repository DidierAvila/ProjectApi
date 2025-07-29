using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Common;
using Business.Customers.Commands;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Business.Customers.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepositoy _customerRepositoy;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(
            ICustomerRepositoy customerRepositoy, 
            IMapper mapper,
            ILogger<CreateCustomerCommandHandler> logger)
        {
            _customerRepositoy = customerRepositoy;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var stopwatch = StructuredLogging.CreateStopwatch();
            
            using var scope = _logger.LogOperationScope<CreateCustomerCommandHandler>("CreateCustomer", new
            {
                CustomerName = request.Name,
                RequestId = Guid.NewGuid().ToString()
            });

            try
            {
                _logger.LogOperationStart<CreateCustomerCommandHandler>("CreateCustomer", new
                {
                    CustomerName = request.Name
                });

                // Validar que el nombre no esté vacío
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogBusinessRuleViolation<CreateCustomerCommandHandler>("CreateCustomer", "CustomerNameRequired", new
                    {
                        CustomerName = request.Name
                    });
                    throw new InvalidOperationException("El nombre del cliente no puede estar vacío");
                }

                // Validar que no exista un customer con el mismo nombre
                var existingCustomer = await _customerRepositoy.Find(c => c.Name.ToLower() == request.Name.ToLower(), cancellationToken);
                if (existingCustomer != null)
                {
                    _logger.LogBusinessRuleViolation<CreateCustomerCommandHandler>("CreateCustomer", "CustomerNameMustBeUnique", new
                    {
                        CustomerName = request.Name,
                        ExistingCustomerId = existingCustomer.CustomerId
                    });
                    throw new InvalidOperationException($"Ya existe un cliente con el nombre '{request.Name}'");
                }

                // Crear el nuevo customer usando AutoMapper
                var customer = _mapper.Map<Domain.Entities.Customer>(request);
                
                var dbStopwatch = StructuredLogging.CreateStopwatch();
                await _customerRepositoy.Create(customer, cancellationToken);
                var dbDuration = dbStopwatch.ElapsedMilliseconds;
                
                _logger.LogDatabaseOperation<CreateCustomerCommandHandler>("CreateCustomer", "INSERT INTO Customer", dbDuration, 1);

                // Mapear la entidad creada a DTO usando AutoMapper
                var result = _mapper.Map<CustomerDto>(customer);
                
                var totalDuration = stopwatch.ElapsedMilliseconds;
                _logger.LogPerformanceWarning<CreateCustomerCommandHandler>("CreateCustomer", totalDuration);
                
                _logger.LogOperationSuccess<CreateCustomerCommandHandler>("CreateCustomer", new
                {
                    CustomerId = result.CustomerId,
                    CustomerName = result.Name
                }, totalDuration);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogOperationError<CreateCustomerCommandHandler>("CreateCustomer", ex, new
                {
                    CustomerName = request.Name
                });
                throw;
            }
        }
    }
} 