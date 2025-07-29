using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Posts.Commands;
using DataAccess.Repositories;
using MediatR;

namespace Business.Posts.Handlers
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IPostRepository _postRepository;

        public DeletePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            // Buscar el post existente
            var post = await _postRepository.GetByID(request.PostId, cancellationToken);
            if (post == null)
            {
                throw new InvalidOperationException($"No se encontró el post con ID {request.PostId}");
            }

            // Eliminar el post
            await _postRepository.Delete(request.PostId, cancellationToken);

            return true;
        }
    }
} 