using System.ComponentModel.DataAnnotations;

namespace WorldForge.Web.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
