using System.ComponentModel.DataAnnotations;

namespace ChatApp.Server.DTOs.Account
{
    public class ConfirmEmailDto
    {
        [Required,RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
