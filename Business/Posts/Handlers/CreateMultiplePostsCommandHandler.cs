using AutoMapper;
using Business.Logs;
using Business.Posts.Commands;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;

namespace Business.Posts.Handlers
{
    public class CreateMultiplePostsCommandHandler : IRequestHandler<CreateMultiplePostsCommand, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICustomerRepositoy _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public CreateMultiplePostsCommandHandler(IPostRepository postRepository, ICustomerRepositoy customerRepository, IMapper mapper, ILogService logService)
        {
            _postRepository = postRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<IEnumerable<PostDto>> Handle(CreateMultiplePostsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Posts == null || !request.Posts.Any())
                {
                    return [new PostDto() { Messages = "No se proporcionaron posts para crear" }];
                }

                var createdPosts = new List<Domain.Entities.Post>();

                foreach (var postRequest in request.Posts)
                {
                    // Validar que el customer exista
                    var customer = await _customerRepository.GetByID(postRequest.CustomerId, cancellationToken);
                    if (customer == null)
                    {
                        return [new PostDto() { Messages = $"No se encontró el cliente con ID {postRequest.CustomerId}" }];
                    }

                    // Validar campos requeridos
                    if (string.IsNullOrWhiteSpace(postRequest.Title))
                    {
                        return [new PostDto() { Messages = "El título del post no puede estar vacío" }];
                    }

                    if (string.IsNullOrWhiteSpace(postRequest.Body))
                    {
                        return [new PostDto() { Messages = "El contenido del post no puede estar vacío" }];
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
            catch (Exception ex)
            {
                await _logService.CreateLog(new Domain.Entities.Logs
                {
                    Message = ex.Message,
                    Level = "Error",
                    TimeStamp = DateTime.UtcNow,
                    Properties = $"",
                    Exception = ex.GetType().Name,
                    MessageTemplate = ex.Source.ToString()
                }, cancellationToken);
                throw;
            } 
        }
    }
} 