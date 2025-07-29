using MediatR;

namespace Business.Posts.Commands
{
    public class DeletePostCommand : IRequest<bool>
    {
        public int PostId { get; set; }
    }
} 