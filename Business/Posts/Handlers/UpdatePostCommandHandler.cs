using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Posts.Commands;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using MediatR;

namespace Business.Posts.Handlers
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICustomerRepositoy _customerRepository;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(IPostRepository postRepository, ICustomerRepositoy customerRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new InvalidOperationException("El título del post no puede estar vacío");
            }

            if (string.IsNullOrWhiteSpace(request.Body))
            {
                throw new InvalidOperationException("El contenido del post no puede estar vacío");
            }

            // Buscar el post existente
            var post = await _postRepository.GetByID(request.PostId, cancellationToken);
            if (post == null)
            {
                throw new InvalidOperationException($"No se encontró el post con ID {request.PostId}");
            }

            // Validar que el customer exista
            var customer = await _customerRepository.GetByID(request.CustomerId, cancellationToken);
            if (customer == null)
            {
                throw new InvalidOperationException($"No se encontró el cliente con ID {request.CustomerId}");
            }

            // Actualizar el post
            post.Title = request.Title.Trim();
            post.Body = request.Body;
            post.Type = request.Type;
            post.CustomerId = request.CustomerId;

            // Procesar el body según las reglas de negocio
            if (post.Body.Length > 20 && post.Body.Length > 97)
            {
                post.Body = post.Body.Substring(0, 97) + "...";
            }

            // Asignar categoría automática según el Type
            post.Category = request.Type switch
            {
                1 => "Farándula",
                2 => "Política",
                3 => "Futbol",
                _ => request.Category
            };

            await _postRepository.Update(post, cancellationToken);

            // Mapear la entidad actualizada a DTO usando AutoMapper
            return _mapper.Map<PostDto>(post);
        }
    }
} 