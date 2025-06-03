using System.ComponentModel.DataAnnotations;

namespace WorldForge.Web.Models
{
    public enum BookGenre
    {
        [Display(Name = "Fantasy")]
        Fantasy,

        [Display(Name = "Science Fiction")]
        ScienceFiction,

        [Display(Name = "Horror")]
        Horror,

        [Display(Name = "Mystery")]
        Mystery,

        [Display(Name = "Thriller")]
        Thriller,

        [Display(Name = "Romance")]
        Romance,

        [Display(Name = "Historical")]
        Historical,

        [Display(Name = "Adventure")]
        Adventure,

        [Display(Name = "Non-Fiction")]
        NonFiction,

        [Display(Name = "Other")]
        Other
    }
}
