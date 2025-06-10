using LIBRARY.Shared.DTO;
using LIBRARY.Shared.Entity;
using Microsoft.AspNetCore.Identity;

namespace BACK_END.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(Guid userId);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<SignInResult> LoginAsync(LoginDTO model);
        Task LogoutAsync();
    }
}