using GameStore.Data;
using GameStore.Extensions;
using GameStore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CartService
{
    private const string CartKey = "Cart";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GameStoreDbContext _db;

    public CartService(IHttpContextAccessor httpContextAccessor, GameStoreDbContext db)
    {
        _httpContextAccessor = httpContextAccessor;
        _db = db;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public List<CartItemViewModel> GetItems()
    {
        return Session.GetObject<List<CartItemViewModel>>(CartKey) ?? new List<CartItemViewModel>();
    }

    public async Task AddAsync(int productId, int quantity = 1)
    {
        var product = await _db.Products
            .Include(p => p.Game)
            .Include(p => p.Platform)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null || !product.IsAvailable)
            return;

        var items = GetItems();
        var existing = items.FirstOrDefault(i => i.ProductId == productId);

        if (existing is not null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            items.Add(new CartItemViewModel
            {
                ProductId = product.Id,
                GameTitle = product.Game.Title,
                PlatformName = product.Platform.Name,
                Price = product.Price,
                Quantity = quantity
            });
        }

        Session.SetObject(CartKey, items);
    }

    public void Remove(int productId)
    {
        var items = GetItems();
        var item = items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            items.Remove(item);
            Session.SetObject(CartKey, items);
        }
    }

    public void Clear()
    {
        Session.Remove(CartKey);
    }

    public int GetTotalCount()
    {
        return GetItems().Sum(i => i.Quantity);
    }

    public decimal GetTotalPrice()
    {
        return GetItems().Sum(i => i.Total);
    }
}