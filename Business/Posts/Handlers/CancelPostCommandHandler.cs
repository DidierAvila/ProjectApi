using AutoMapper;
using Business.Common;
using Business.Logs;
using Business.Posts.Commands;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Business.Posts.Handlers
{
    public class CancelPostCommandHandler : IRequestHandler<CancelPostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelPostCommandHandler> _logger;
        private readonly ILogService _logService;
        
        public CancelPostCommandHandler(
            IPostRepository postRepository, 
            IMapper mapper,
            ILogger<CancelPostCommandHandler> logger,
            ILogService logService)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _logger = logger;
            _logService = logService;
        }

        public async Task<PostDto> Handle(CancelPostCommand request, CancellationToken cancellationToken)
        {
            var stopwatch = StructuredLogging.CreateStopwatch();
            
            using var scope = _logger.LogOperationScope<CancelPostCommandHandler>("CancelPost", new
            {
                PostId = request.PostId,
                RequestId = Guid.NewGuid().ToString()
            });

            try
            {
                _logger.LogOperationStart<CancelPostCommandHandler>("CancelPost", new
                {
                    PostId = request.PostId
                });

                // Verificar que el post existe
                var post = await _postRepository.GetByID(request.PostId, cancellationToken);
                if (post == null)
                {
                    _logger.LogBusinessRuleViolation<CancelPostCommandHandler>("CancelPost", "PostNotFound", new
                    {
                        PostId = request.PostId
                    });
                    return new PostDto { Messages = $"No se encontró el post con ID {request.PostId}" };
                }

                // Verificar que el post no esté ya cancelado
                if (post.Status == EntityStatus.Cancelled)
                {
                    _logger.LogBusinessRuleViolation<CancelPostCommandHandler>("CancelPost", "PostAlreadyCancelled", new
                    {
                        PostId = request.PostId,
                        CurrentStatus = post.Status
                    });
                    return new PostDto { Messages = $"El post con ID {request.PostId} ya está cancelado" };
                }

                // Cancelar el post
                post.Status = EntityStatus.Cancelled;
                
                var dbStopwatch = StructuredLogging.CreateStopwatch();
                await _postRepository.Update(post, cancellationToken);
                var dbDuration = dbStopwatch.ElapsedMilliseconds;
                
                _logger.LogDatabaseOperation<CancelPostCommandHandler>("CancelPost", "UPDATE Post SET Status = Cancelled", dbDuration, 1);

                // Mapear la entidad actualizada a DTO
                var result = _mapper.Map<PostDto>(post);
                
                var totalDuration = stopwatch.ElapsedMilliseconds;
                _logger.LogPerformanceWarning<CancelPostCommandHandler>("CancelPost", totalDuration);
                
                _logger.LogOperationSuccess<CancelPostCommandHandler>("CancelPost", new
                {
                    PostId = result.PostId,
                    NewStatus = result.Status
                }, totalDuration);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogOperationError<CancelPostCommandHandler>("CancelPost", ex, new
                {
                    PostId = request.PostId
                });
                await _logService.CreateLog(new Domain.Entities.Logs
                {
                    Message = ex.Message,
                    Level = "Error",
                    TimeStamp = DateTime.UtcNow,
                    Properties = $"PostId: {request.PostId}",
                    Exception = ex.GetType().Name,
                    MessageTemplate = "Error cancelling post"
                }, cancellationToken);
                throw;
            }
        }
    }
}