namespace labapp.Models;

public class DashboardViewModel
{
    public long Total { get; set; }
    public List<KeyValuePair<string, long>> TopPaths { get; set; } = new();
}