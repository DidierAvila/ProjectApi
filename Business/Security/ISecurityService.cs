using Domain.Dtos.Security;
using System.Threading.Tasks;
using System.Threading;

namespace Business.Security
{
    public interface ISecurityService
    {
        Task<LoginResponse?> Login(LoginRequest autorizacion, CancellationToken cancellationToken);
    }
}