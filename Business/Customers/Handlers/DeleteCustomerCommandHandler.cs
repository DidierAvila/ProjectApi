using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Customers.Commands;
using DataAccess.Repositories;
using MediatR;

namespace Business.Customers.Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepositoy _customerRepositoy;
        private readonly IPostRepository _postRepository;

        public DeleteCustomerCommandHandler(ICustomerRepositoy customerRepositoy, IPostRepository postRepository)
        {
            _customerRepositoy = customerRepositoy;
            _postRepository = postRepository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            // Buscar el customer existente
            var customer = await _customerRepositoy.GetByID(command.CustomerId, cancellationToken);
            if (customer == null)
            {
                throw new InvalidOperationException($"No se encontró el cliente con ID {command.CustomerId}");
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
            
            return true;
        }
    }
} 