using System.Net.Mail;
using System.Text.RegularExpressions;
using BLL.Abstractions.ServiceInterfaces;
using BLL.Infrastructure;
using Core.DTO_Models;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;
public class UserService : IUserService
{
    private readonly GigienaStoreDbContext _context;

    public UserService(GigienaStoreDbContext context)
    {
        _context = context;
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

        if (await _context.Users.AnyAsync(us=> us.Email == user.Email))
        {
            return false;
        }

        var userId = Guid.NewGuid();
        var crUser = new User
        {
            UserId = userId,
            Email = user.Email,
            HashPassword = PasswordHasher.HashPassword(user.Password)
        };

        try
        {
            if (user.Specialty == "Customer")
            {
                var customer = new Customer
                {
                    CustomerId = userId,
                    Name = user.Name,
                    Surname = user.Surname,
                    MiddleName = user.MiddleName,
                    Sex = user.Sex,
                    Dob = user.Dob,
                    Dor = DateTime.Now,
                    City = user.City,
                    Address = user.Address,
                    Zip = user.Zip.Value,
                    Phone = user.Phone
                };
                await _context.Customers.AddAsync(customer);
            }
            else
            {
                var worker = new Worker
                {
                    WorkerId = userId,
                    Name = user.Name,
                    Surname = user.Surname,
                    MiddleName = user.MiddleName,
                    Sex = user.Sex,
                    Dob = user.Dob,
                    City = user.City,
                    Address = user.Address,
                    Specialty = user.Specialty,
                    Salary = user.Salary.Value,
                    ShopId = user.ShopId
                };
                await _context.Workers.AddAsync(worker);
            }
            await _context.Users.AddAsync(crUser);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<UserAfterLogin> SignInAsync(string username, string password)
    {
        var passwordRegex = new Regex(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9]).{8,24}",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        var foundUser = await _context.Users.Include(x => x.Customer).Include(x => x.Worker).FirstOrDefaultAsync(x => x.Email == username);

        if (foundUser is null)
        {
            return null;
        }

        var result = new UserAfterLogin
        {
            UserId = foundUser.UserId,
            Role = foundUser.Customer is null ? foundUser.Worker.Specialty : "Customer"
        };
        
        if (!passwordRegex.IsMatch(password) || !PasswordHasher.VerifyHashedPassword(foundUser.HashPassword, password)) // check if password is strong
        {
            throw new ValidationException("Incorrect password", "Password");
        }

        return result;
    }
}