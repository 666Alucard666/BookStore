

namespace Core.DTO_Models
{
    public class UserAfterLogin
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string CancelDate { get; set; }
        public string Role { get; set; }
    }
}
