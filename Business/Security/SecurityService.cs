using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Repositories;
using Domain.Dtos.Security;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Business.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryBase<Token> _TokenRepository;
        private readonly IRepositoryBase<User> _UserRepository;
        private readonly ILogger<SecurityService> _logger;

        public SecurityService(IConfiguration configuration, IRepositoryBase<Token> tokenRepository, IRepositoryBase<User> userRepository, ILogger<SecurityService> logger)
        {
            _configuration = configuration;
            _TokenRepository = tokenRepository;
            _UserRepository = userRepository;
            _logger = logger;
        }

        public async Task<LoginResponse?> Login(LoginRequest autorizacion, CancellationToken cancellationToken)
        {
            User? CurrentUser = await _UserRepository.Find(x => x.Email == autorizacion.UserName && x.Password == autorizacion.Password, cancellationToken);
            if (CurrentUser != null)
            {
                _logger.LogInformation("Login: succes");
                string CurrentToken = await GetToken(CurrentUser, cancellationToken);
                LoginResponse loginResponse = new()
                { Token = CurrentToken };

                return loginResponse;
            }
            return null;
        }

        private async Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken)
        {
            string? key = _configuration.GetValue<string>("JwtSettings:key");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            // Crear el token
            DateTime ExperiredDate = DateTime.Now.AddMinutes(60);
            JwtSecurityToken tokenJwt = new(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: ExperiredDate,
                signingCredentials: credentials
            );

            string Newtoken = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

            //Se almacena el nuevo token
            if (!string.IsNullOrEmpty(Newtoken))
            {
                Token token = new()
                {
                    TokenValue = Newtoken,
                    Status = true,
                    IdUsuario = user.Id,
                    CreatedDate = DateTime.Now,
                    ExpirationDate = ExperiredDate
                };

                await _TokenRepository.Create(token, cancellationToken);
            }
            return Newtoken;
        }

        private async Task<string> RefreshTokenAsync(Token token, User user, CancellationToken cancellationToken)
        {
            token.Status = false;
            await _TokenRepository.Update(token, cancellationToken);

            string currentToken = await GenerateTokenAsync(user, cancellationToken);
            return currentToken;
        }

        private async Task<string> GetToken(User user, CancellationToken cancellationToken)
        {
            var CurrentToken = await _TokenRepository.Find(x => x.IdUsuario == user.Id && x.Status, cancellationToken);
            if (CurrentToken != null)
            {
                if (CurrentToken.ExpirationDate.CompareTo(DateTime.Now) < 0)
                {
                    _logger.LogInformation("GetToken: Expiration Token UserId:" + user.Id);
                    return await RefreshTokenAsync(CurrentToken, user, cancellationToken);
                }
                return CurrentToken.TokenValue!;
            }
            else
            {
                string currentToken = await GenerateTokenAsync(user, cancellationToken);
                if (!string.IsNullOrEmpty(currentToken))
                {
                    return currentToken;
                }
            }
            return string.Empty;
        }
    }
}
