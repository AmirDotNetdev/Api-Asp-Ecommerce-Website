using TestApi.DTOs.ShoppingCartDto;

namespace TestApi.Repositories.ShoppingCartRepository
{
    public interface IShoppingCartRepository
    {
        public Task<Response_ShoppingCart> GetAllCartItems(string userId);
        public Task<Response_ShoppingCartInfo> AddItem(string userId, Request_ShoppingCart shoppingCartItem);
        public Task<Response_ShoppingCartInfo> UpdateQty(string userId, Request_ShoppingCart shoppingCartItem);
        public Task<Response_ShoppingCartInfo> RemoveItem(string userId, Request_ShoppingCart shoppingCartItem);
        public Task<Response_ShoppingCartInfo>ClearCart(string userId);

    }
}
