using GameStore.Data;
using GameStore.Models;
using GameStore.Services;
using GameStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Controllers;

public class CatalogController : Controller
{
    private readonly GameStoreDbContext _db;
    private readonly CartService _cart;

    public CatalogController(GameStoreDbContext db, CartService cart)
    {
        _db = db;
        _cart = cart;
    }

    public async Task<IActionResult> Index(int? platformId, int? genreId)
    {
        var query = _db.Products
            .Include(p => p.Game)
                .ThenInclude(g => g.Images)
            .Include(p => p.Game)
                .ThenInclude(g => g.GameGenres)
            .Include(p => p.Platform)
            .Where(p => p.IsAvailable)
            .AsQueryable();

        if (platformId.HasValue)
            query = query.Where(p => p.PlatformId == platformId.Value);

        if (genreId.HasValue)
            query = query.Where(p => p.Game.GameGenres.Any(gg => gg.GenreId == genreId.Value));

        var vm = new CatalogViewModel
        {
            Products = await query.ToListAsync(),
            Platforms = await _db.Platforms.ToListAsync(),
            Genres = await _db.Genres.ToListAsync(),
            SelectedPlatformId = platformId,
            SelectedGenreId = genreId
        };

        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products
            .Include(p => p.Game)
                .ThenInclude(g => g.Images)
            .Include(p => p.Game)
                .ThenInclude(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
            .Include(p => p.Game)
                .ThenInclude(g => g.GamePublishers)
                    .ThenInclude(gp => gp.Publisher)
            .Include(p => p.Game)
                .ThenInclude(g => g.GameDevelopers)
                    .ThenInclude(gd => gd.Developer)
            .Include(p => p.Platform)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return NotFound();

        return View(product);
    }
}