using System.ComponentModel.DataAnnotations;

namespace WorldForge.Web.Models
{
    public enum CharacterStatus
    {
        [Display(Name = "Active")]
        Active,

        [Display(Name = "Inactive")]
        Inactive,

        [Display(Name = "Deceased")]
        Deceased,

        [Display(Name = "Missing")]
        Missing
    }

}
