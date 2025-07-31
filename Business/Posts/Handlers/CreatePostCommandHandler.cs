using AutoMapper;
using Business.Common;
using Business.Posts.Commands;
using DataAccess.Repositories;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Business.Posts.Handlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICustomerRepositoy _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePostCommandHandler> _logger;

        public CreatePostCommandHandler(
            IPostRepository postRepository, 
            ICustomerRepositoy customerRepository, 
            IMapper mapper,
            ILogger<CreatePostCommandHandler> logger)
        {
            _postRepository = postRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var stopwatch = StructuredLogging.CreateStopwatch();
            
            using var scope = _logger.LogOperationScope<CreatePostCommandHandler>("CreatePost", new
            {
                PostTitle = request.Title,
                CustomerId = request.CustomerId,
                PostType = request.Type,
                RequestId = Guid.NewGuid().ToString()
            });

            try
            {
                _logger.LogOperationStart<CreatePostCommandHandler>("CreatePost", new
                {
                    PostTitle = request.Title,
                    CustomerId = request.CustomerId,
                    PostType = request.Type,
                    Category = request.Category
                });

                // Validar que el customer exista
                var customerStopwatch = StructuredLogging.CreateStopwatch();
                var customer = await _customerRepository.GetByID(request.CustomerId, cancellationToken);
                var customerLookupDuration = customerStopwatch.ElapsedMilliseconds;
                
                _logger.LogDatabaseOperation<CreatePostCommandHandler>("CustomerLookup", 
                    $"SELECT * FROM Customer WHERE CustomerId = {request.CustomerId}", 
                    customerLookupDuration);

                if (customer == null)
                {
                    _logger.LogBusinessRuleViolation<CreatePostCommandHandler>("CreatePost", "CustomerMustExist", new
                    {
                        CustomerId = request.CustomerId
                    });
                    throw new InvalidOperationException($"No se encontró el cliente con ID {request.CustomerId}");
                }

                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    _logger.LogBusinessRuleViolation<CreatePostCommandHandler>("CreatePost", "PostTitleRequired", new
                    {
                        PostTitle = request.Title
                    });
                    throw new InvalidOperationException("El título del post no puede estar vacío");
                }

                if (string.IsNullOrWhiteSpace(request.Body))
                {
                    _logger.LogBusinessRuleViolation<CreatePostCommandHandler>("CreatePost", "PostBodyRequired", new
                    {
                        PostBody = request.Body
                    });
                    throw new InvalidOperationException("El contenido del post no puede estar vacío");
                }

                // Crear el post usando AutoMapper
                var post = _mapper.Map<Domain.Entities.Post>(request);

                // Procesar el body según las reglas de negocio
                var originalBodyLength = post.Body.Length;
                if (post.Body.Length > 20 && post.Body.Length > 97)
                {
                    post.Body = post.Body.Substring(0, 97) + "...";
                    _logger.LogInformation("Post body truncated: {OriginalLength} -> {NewLength}", originalBodyLength, post.Body.Length);
                    _logger.LogInformation("Truncation details: {@TruncationDetails}", new
                    {
                        OriginalLength = originalBodyLength,
                        NewLength = post.Body.Length,
                        Truncated = true
                    });
                }

                // Asignar categoría automática según el Type
                var originalCategory = post.Category;
                post.Category = request.Type switch
                {
                    1 => "Farándula",
                    2 => "Política",
                    3 => "Futbol",
                    _ => request.Category
                };

                if (originalCategory != post.Category)
                {
                    _logger.LogInformation("Post category auto-assigned: {OriginalCategory} -> {NewCategory}", originalCategory, post.Category);
                    _logger.LogInformation("Category assignment details: {@CategoryDetails}", new
                    {
                        PostType = request.Type,
                        OriginalCategory = originalCategory,
                        NewCategory = post.Category,
                        AutoAssigned = true
                    });
                }

                // Crear el post en la base de datos
                var dbStopwatch = StructuredLogging.CreateStopwatch();
                await _postRepository.Create(post, cancellationToken);
                var dbDuration = dbStopwatch.ElapsedMilliseconds;
                
                _logger.LogDatabaseOperation<CreatePostCommandHandler>("CreatePost", "INSERT INTO Post", dbDuration, 1);

                // Mapear la entidad creada a DTO usando AutoMapper
                var result = _mapper.Map<PostDto>(post);
                
                var totalDuration = stopwatch.ElapsedMilliseconds;
                _logger.LogPerformanceWarning<CreatePostCommandHandler>("CreatePost", totalDuration);
                
                _logger.LogOperationSuccess<CreatePostCommandHandler>("CreatePost", new
                {
                    PostId = result.PostId,
                    PostTitle = result.Title,
                    CustomerId = result.CustomerId,
                    Category = result.Category,
                    Type = result.Type
                }, totalDuration);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogOperationError<CreatePostCommandHandler>("CreatePost", ex, new
                {
                    PostTitle = request.Title,
                    CustomerId = request.CustomerId,
                    PostType = request.Type
                });
                throw;
            }
        }
    }
} 