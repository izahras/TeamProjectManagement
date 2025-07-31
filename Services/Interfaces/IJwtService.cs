using TeamProjectManagement.DTOs;
using TeamProjectManagement.Models;

namespace TeamProjectManagement.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        Task<AuthResponseDto> GenerateAuthResponseAsync(User user);
        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        int? GetUserIdFromToken(string token);
    }
}