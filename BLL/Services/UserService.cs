using System.Net.Mail;
using System.Text.RegularExpressions;
using BLL.Abstractions.ServiceInterfaces;
using BLL.Infrastructure;
using Core.DTO_Models;
using Core.Models;
using DAL.Abstractions.Interfaces;

namespace BLL.Services;
public class UserService : IUserService
{
    private IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork work)
    {
        _unitOfWork = work;
    }
    public async Task<bool> RegisterAsync(UserDTO user)
    {
        try
        {
            var checkedEmail = new MailAddress(user.Email).Address; // check if email string is in a email format
        }
        catch (FormatException e)
        {
            return false;
        }

        // Password must contain numbers, lowercase or uppercase letters, include special symbols, at least 8 characters, at most 24 characters.
        var passwordRegex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,24}",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        if (!passwordRegex.IsMatch(user.Password)) // check if password is strong
        {
            return false;
        }

        if (await _unitOfWork.UserRepository.Any(us=> us.Email == user.Email ||
            user.UserName == us.Username || us.PhoneNumber == user.PhoneNumber))
        {
            return false;
        }
        var crUser = new User
        {
            Email = user.Email,
            Username = user.UserName,
            Password = user.Password,
            PhoneNumber = user.PhoneNumber,
            Name = user.Name,
            Surname = user.Surname,
            Role = user.Role,
            Orders = new List<Order>(),
        };
        crUser.Password = PasswordHasher.HashPassword(crUser.Password);
        using (_unitOfWork.BeginTransactionAsync())
        {
            try
            { 
                _unitOfWork.UserRepository.Create(crUser);
                await _unitOfWork.SaveAsync();
                    
                await _unitOfWork.CommitTransactionAsync();
            }
            catch 
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
        }

        return true;
    }

    public async Task<User> SignInAsync(string username, string password)
    {
        var passwordRegex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,24}",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        if (!passwordRegex.IsMatch(password)) // check if password is strong
        {
            throw new ValidationException("Incorrect password", "Password");
        }

        if (!await _unitOfWork.UserRepository.Any())
        {
            return null;
        }

        User foundUser = null;
        foundUser = _unitOfWork.UserRepository.FirstOrDefault(u => u.Username == username);

        if (foundUser == null
            || !PasswordHasher.VerifyHashedPassword(foundUser.Password, password))
        {
            return null;
        }

        return foundUser;
    }
}