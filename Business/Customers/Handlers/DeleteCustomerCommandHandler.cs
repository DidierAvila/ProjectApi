using Business.Customers.Commands;
using Business.Logs;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Customers.Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepositoy _customerRepositoy;
        private readonly IPostRepository _postRepository;
        private readonly ILogService _logService;

        public DeleteCustomerCommandHandler(ICustomerRepositoy customerRepositoy, IPostRepository postRepository, ILogService logService)
        {
            _customerRepositoy = customerRepositoy;
            _postRepository = postRepository;
            _logService = logService;
        }

        public async Task<CustomerDto> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Buscar el customer existente
                var customer = await _customerRepositoy.GetByID(command.CustomerId, cancellationToken);
                if (customer == null)
                {
                    return new CustomerDto() { Messages = $"No se encontró el cliente con ID {command.CustomerId}" };
                }

                // Eliminar todos los posts asociados al customer
                var customerPosts = await _postRepository.Finds(p => p.CustomerId == command.CustomerId, cancellationToken);
                if (customerPosts != null && customerPosts.Any())
                {
                    foreach (var post in customerPosts)
                    {
                        await _postRepository.Delete(post.PostId, cancellationToken);
                    }
                }

                // Eliminar el customer
                await _customerRepositoy.Delete(command.CustomerId, cancellationToken);

                return new CustomerDto() { CustomerId = command.CustomerId };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(new Domain.Entities.Logs
                {
                    Message = ex.Message,
                    Level = "Validation",
                    TimeStamp = DateTime.UtcNow,
                    Properties = $"{{ \"CustomerId\": {command.CustomerId} }}",
                    Exception = ex.GetType().Name,
                    MessageTemplate = ex.StackTrace
                }, cancellationToken);
                throw;
            }
            
        }
    }
} 