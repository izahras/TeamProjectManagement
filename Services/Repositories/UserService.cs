using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TeamProjectManagement.Data;
using TeamProjectManagement.DTOs;
using TeamProjectManagement.Models;
using TeamProjectManagement.Services.Interfaces;

namespace TeamProjectManagement.Services.Repositories
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Check if email or username already exists
            if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email))
                throw new InvalidOperationException("Email already exists");

            if (await _context.Users.AnyAsync(u => u.Username == createUserDto.Username))
                throw new InvalidOperationException("Username already exists");

            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            if (updateUserDto.FirstName != null)
                user.FirstName = updateUserDto.FirstName;

            if (updateUserDto.LastName != null)
                user.LastName = updateUserDto.LastName;

            if (updateUserDto.Email != null)
            {
                if (await _context.Users.AnyAsync(u => u.Email == updateUserDto.Email && u.Id != id))
                    throw new InvalidOperationException("Email already exists");
                user.Email = updateUserDto.Email;
            }

            if (updateUserDto.Username != null)
            {
                if (await _context.Users.AnyAsync(u => u.Username == updateUserDto.Username && u.Id != id))
                    throw new InvalidOperationException("Username already exists");
                user.Username = updateUserDto.Username;
            }

            if (updateUserDto.Role.HasValue)
                user.Role = updateUserDto.Role.Value;

            if (updateUserDto.IsActive.HasValue)
                user.IsActive = updateUserDto.IsActive.Value;

            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int role)
        {
            var users = await _context.Users
                .Where(u => (int)u.Role == role && u.IsActive)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}