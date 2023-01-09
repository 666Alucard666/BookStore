using Core.DTO_Models;
namespace BLL.Abstractions.ServiceInterfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(UserDTO user);
        Task<UserAfterLogin> SignInAsync(string username, string password);
    }
}
