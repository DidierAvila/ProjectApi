using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CreateMultiplePostsCommandHandler : IRequestHandler<CreateMultiplePostsCommand, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICustomerRepositoy _customerRepository;
        private readonly IMapper _mapper;

        public CreateMultiplePostsCommandHandler(IPostRepository postRepository, ICustomerRepositoy customerRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> Handle(CreateMultiplePostsCommand request, CancellationToken cancellationToken)
        {
            if (request.Posts == null || !request.Posts.Any())
            {
                throw new InvalidOperationException("No se proporcionaron posts para crear");
            }

            var createdPosts = new List<Domain.Entities.Post>();

            foreach (var postRequest in request.Posts)
            {
                // Validar que el customer exista
                var customer = await _customerRepository.GetByID(postRequest.CustomerId, cancellationToken);
                if (customer == null)
                {
                    throw new InvalidOperationException($"No se encontró el cliente con ID {postRequest.CustomerId}");
                }

                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(postRequest.Title))
                {
                    throw new InvalidOperationException("El título del post no puede estar vacío");
                }

                if (string.IsNullOrWhiteSpace(postRequest.Body))
                {
                    throw new InvalidOperationException("El contenido del post no puede estar vacío");
                }

                // Crear el post usando AutoMapper
                var post = _mapper.Map<Domain.Entities.Post>(postRequest);

                // Procesar el body según las reglas de negocio
                if (post.Body.Length > 20 && post.Body.Length > 97)
                {
                    post.Body = post.Body.Substring(0, 97) + "...";
                }

                // Asignar categoría automática según el Type
                post.Category = postRequest.Type switch
                {
                    1 => "Farándula",
                    2 => "Política",
                    3 => "Futbol",
                    _ => postRequest.Category
                };

                await _postRepository.Create(post, cancellationToken);
                createdPosts.Add(post);
            }

            // Mapear las entidades creadas a DTOs usando AutoMapper
            return _mapper.Map<IEnumerable<PostDto>>(createdPosts);
        }
    }
} 