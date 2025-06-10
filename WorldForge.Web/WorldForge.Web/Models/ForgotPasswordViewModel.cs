using System.ComponentModel.DataAnnotations;

namespace WorldForge.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
