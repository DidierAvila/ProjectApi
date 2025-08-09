using AutoMapper;
using Business.Common;
using Business.Customers.Commands;
using Business.Logs;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Business.Customers.Handlers
{
    public class CancelCustomerCommandHandler : IRequestHandler<CancelCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepositoy _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelCustomerCommandHandler> _logger;
        private readonly ILogService _logService;
        
        public CancelCustomerCommandHandler(
            ICustomerRepositoy customerRepository, 
            IMapper mapper,
            ILogger<CancelCustomerCommandHandler> logger,
            ILogService logService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
            _logService = logService;
        }

        public async Task<CustomerDto> Handle(CancelCustomerCommand request, CancellationToken cancellationToken)
        {
            var stopwatch = StructuredLogging.CreateStopwatch();
            
            using var scope = _logger.LogOperationScope<CancelCustomerCommandHandler>("CancelCustomer", new
            {
                CustomerId = request.CustomerId,
                RequestId = Guid.NewGuid().ToString()
            });

            try
            {
                _logger.LogOperationStart<CancelCustomerCommandHandler>("CancelCustomer", new
                {
                    CustomerId = request.CustomerId
                });

                // Verificar que el cliente existe
                var customer = await _customerRepository.GetByID(request.CustomerId, cancellationToken);
                if (customer == null)
                {
                    _logger.LogBusinessRuleViolation<CancelCustomerCommandHandler>("CancelCustomer", "CustomerNotFound", new
                    {
                        CustomerId = request.CustomerId
                    });
                    return new CustomerDto { Messages = $"No se encontró el cliente con ID {request.CustomerId}" };
                }

                // Verificar que el cliente no esté ya cancelado
                if (customer.Status == EntityStatus.Cancelled)
                {
                    _logger.LogBusinessRuleViolation<CancelCustomerCommandHandler>("CancelCustomer", "CustomerAlreadyCancelled", new
                    {
                        CustomerId = request.CustomerId,
                        CurrentStatus = customer.Status
                    });
                    return new CustomerDto { Messages = $"El cliente con ID {request.CustomerId} ya está cancelado" };
                }

                // Cancelar el cliente
                customer.Status = EntityStatus.Cancelled;
                
                var dbStopwatch = StructuredLogging.CreateStopwatch();
                await _customerRepository.Update(customer, cancellationToken);
                var dbDuration = dbStopwatch.ElapsedMilliseconds;
                
                _logger.LogDatabaseOperation<CancelCustomerCommandHandler>("CancelCustomer", "UPDATE Customer SET Status = Cancelled", dbDuration, 1);

                // Mapear la entidad actualizada a DTO
                var result = _mapper.Map<CustomerDto>(customer);
                
                var totalDuration = stopwatch.ElapsedMilliseconds;
                _logger.LogPerformanceWarning<CancelCustomerCommandHandler>("CancelCustomer", totalDuration);
                
                _logger.LogOperationSuccess<CancelCustomerCommandHandler>("CancelCustomer", new
                {
                    CustomerId = result.CustomerId,
                    NewStatus = result.Status
                }, totalDuration);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogOperationError<CancelCustomerCommandHandler>("CancelCustomer", ex, new
                {
                    CustomerId = request.CustomerId
                });
                await _logService.CreateLog(new Domain.Entities.Logs
                {
                    Message = ex.Message,
                    Level = "Error",
                    TimeStamp = DateTime.UtcNow,
                    Properties = $"CustomerId: {request.CustomerId}",
                    Exception = ex.GetType().Name,
                    MessageTemplate = "Error cancelling customer"
                }, cancellationToken);
                throw;
            }
        }
    }
}