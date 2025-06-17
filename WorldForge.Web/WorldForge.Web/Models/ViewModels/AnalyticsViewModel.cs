namespace WorldForge.Web.Models.ViewModels
{
    public class AnalyticsViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalCharacters { get; set; }
        public int TotalWorldNotes { get; set; }
        public int[] MonthlyActiveUsers { get; set; } // Can be bound to a chart
    }
}
