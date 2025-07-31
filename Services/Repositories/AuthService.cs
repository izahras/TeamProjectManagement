using Microsoft.EntityFrameworkCore;
using TeamProjectManagement.Data;
using TeamProjectManagement.DTOs;
using TeamProjectManagement.Models;
using TeamProjectManagement.Services.Interfaces;

namespace TeamProjectManagement.Services.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => (u.Email == loginDto.Email ||  u.Username==loginDto.UserName) && u.IsActive);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            return await _jwtService.GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new InvalidOperationException("Email already exists");

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                throw new InvalidOperationException("Username already exists");

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = Enums.UserRole.Developer, // Default role for new registrations
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await _jwtService.GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (!await _jwtService.ValidateRefreshTokenAsync(refreshToken))
                throw new UnauthorizedAccessException("Invalid refresh token");

            var tokenEntity = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity?.User == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            // Revoke the old refresh token
            await _jwtService.RevokeRefreshTokenAsync(refreshToken);

            // Generate new tokens
            return await _jwtService.GenerateAuthResponseAsync(tokenEntity.User);
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            await _jwtService.RevokeRefreshTokenAsync(refreshToken);
            return true;
        }
    }
}