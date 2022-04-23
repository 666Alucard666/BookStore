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
    public async Task<bool> RegisterAsync(User user)
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
            user.Login == us.Login))
        {
            return false;
        }

        user.Password = PasswordHasher.HashPassword(user.Password);
        using (_unitOfWork.BeginTransactionAsync())
        {
            try
            { 
                _unitOfWork.UserRepository.Create(user);
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
        //TODO: Is phone number or email 
        if (!await _unitOfWork.UserRepository.Any())
        {
            return null;
        }

        User foundUser = null;
        foundUser = _unitOfWork.UserRepository.FirstOrDefault(u => u.Login == username);

        if (foundUser == null
            || !PasswordHasher.VerifyHashedPassword(foundUser.Password, password))
        {
            return null;
        }

        return foundUser;
    }

    public async Task<bool> ChangeUserDataAsync(User user, UserDTO newUserData)
    {
        if (newUserData.Email != null)
        {
            user.Email = newUserData.Email;
        }
        var passwordRegex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,24}",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        if (!passwordRegex.IsMatch(newUserData.Password)|| newUserData.Password == null) // check if password is strong
        {
            throw new ValidationException("Incorrect password", "Password");
        }
        else
        {
            user.Password = newUserData.Password;
            user.Password = PasswordHasher.HashPassword(user.Password);
        }

        if (newUserData.UserName != null)
        {
            user.Login = newUserData.UserName;
        }
        using (_unitOfWork.BeginTransactionAsync())
        {
            try
            { 
                _unitOfWork.UserRepository.Update(user);
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
}