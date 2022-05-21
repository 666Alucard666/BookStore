using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO_Models
{
    public class UserAfterLogin
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
