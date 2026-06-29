namespace InventoryManagement.ViewModels.Search
{
    public class GlobalSearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public List<GlobalSearchResultViewModel> Results { get; set; } = new();

        public bool HasQuery => !string.IsNullOrWhiteSpace(Query);
        public int TotalResults => Results.Count;
    }
}
