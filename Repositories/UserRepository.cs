using ContactsManage.Data;
using Microsoft.EntityFrameworkCore;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.Services;

namespace MyContactsAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly JwtTokenService _jwtTokenService;

    public UserRepository(DataContext dataContext, JwtTokenService jwtTokenService)
    {
        this._dataContext = dataContext;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ApiResponse> UpdateUserAsync(UpdateUserDto userUpdateDto)
    {
        try
        {
            var userIdClaim = _jwtTokenService.GetUserIdFromJwtToken();
            var userToUpdate = await _dataContext.Users.FindAsync(Guid.Parse(userIdClaim));

            if (userToUpdate == null)
            {
                return new ApiResponse("User not found.", 404);
            }

            userToUpdate.Name = userUpdateDto.Name;
            userToUpdate.Username = userUpdateDto.Username;
            userToUpdate.UpdateDate = DateTimeOffset.UtcNow;

            await _dataContext.SaveChangesAsync();

            return new ApiResponse("User updated successfully.", 201);
        }
        catch
        {
            return new ApiResponse("Failed to update user.", 500);
        }
    }

    public async Task<ApiResponse> DeleteUserAsync()
    {
        try
        {
            var userIdClaim = _jwtTokenService.GetUserIdFromJwtToken();

            var userToDelete = await _dataContext.Users.FindAsync(Guid.Parse(userIdClaim));

            if (userToDelete == null)
            {
                return new ApiResponse("User not found.", 404);
            }

            _dataContext.Users.Remove(userToDelete);
            await _dataContext.SaveChangesAsync();

            return new ApiResponse("User deleted successfully.", 200);
        }
        catch
        {
            return new ApiResponse("Failed to delete user.", 500);
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Address == email);
    }
}