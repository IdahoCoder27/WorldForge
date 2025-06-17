namespace WorldForge.Web.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsLockedOut { get; set; }

        public List<string> SelectedRoles { get; set; } = new();

        public List<string> AllRoles { get; set; } = new();
    }
}
