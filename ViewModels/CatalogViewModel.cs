using GameStore.Models;

namespace GameStore.ViewModels;

public class CatalogViewModel
{
    public List<Product> Products { get; set; } = new();

    public List<Platform> Platforms { get; set; } = new();
    public List<Genre> Genres { get; set; } = new();

    public int? SelectedPlatformId { get; set; }
    public int? SelectedGenreId { get; set; }
}