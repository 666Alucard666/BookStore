using Core.DTO_Models;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.ServiceInterfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(User user);
        Task<User> SignInAsync(string username, string password);
        Task<bool> ChangeUserDataAsync(User user, UserDTO newUserData);
    }
}
