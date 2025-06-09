using System.ComponentModel.DataAnnotations;

namespace WorldForge.Web.Models
{
    public class ResetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
