using Microsoft.EntityFrameworkCore;
using TestApi.Data;
using TestApi.DtoConvertations;
using TestApi.DTOs.ShoppingCartDto;
using TestApi.Models.ShoppingCartModels;

namespace TestApi.Repositories.ShoppingCartRepository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ShoppingCartRepository(ApplicationDbContext db)
        {
            _dbContext = db;
        }
        public async Task<Response_ShoppingCart> GetAllCartItems(string userId)
        {
            bool anyCantBeSold = false;
            decimal totalPrice = 0;

            var userCart = await _dbContext.ShoppingCarts.Where(x => x.ApiUserId == userId).FirstOrDefaultAsync();
            if (userCart == null)
            {
                return new Response_ShoppingCart
                {
                    CanBeSold = false,
                    TotalPrice = totalPrice,
                    Message = "no cart found for this user"
                };
            }
            Response_ShoppingCart response_ShoppingCart = new Response_ShoppingCart();
            response_ShoppingCart.ShoppingCartId = userCart.Id;
            var userCartItems = userCart.cartItems.ToList(); 
            foreach(var item in userCartItems)
            {
                Model_CartItemReturn cartItemReturn = new Model_CartItemReturn();
                var existingProductVariant = await _dbContext.ProductVariants.FirstOrDefaultAsync(x => x.Id == item.ProductVariantId);
                if (existingProductVariant == null)
                {
                    cartItemReturn.ProductVariantId = item.ProductVariantId;
                    cartItemReturn.CanBeSold = false;
                    cartItemReturn.SelectedQuantity = item.Quantity;
                    cartItemReturn.Message = "No item found with this given id";
                    response_ShoppingCart.ItemsCantBeSold.Add(cartItemReturn);

                    anyCantBeSold = true;
                }
                else
                {
                    var baseProduct = await _dbContext.BaseProducts.FirstOrDefaultAsync(x => x.Id == existingProductVariant.BaseProductId);

                    cartItemReturn = baseProduct.ConvertToDtoCartItem(existingProductVariant, item);
                    if(cartItemReturn.CanBeSold == false)
                    {
                        response_ShoppingCart.ItemsCantBeSold.Add(cartItemReturn);
                    }
                    else
                    {
                        totalPrice += cartItemReturn.TotalPrice;
                        response_ShoppingCart.Items.Add(cartItemReturn);
                    }
                }
            }
            if(anyCantBeSold == true)
            {
                response_ShoppingCart.CanBeSold = false;
                response_ShoppingCart.Message = "Some Items Cant Be sold";
                response_ShoppingCart.TotalPrice = 0;
            }
            else
            {
                response_ShoppingCart.CanBeSold = true;
                response_ShoppingCart.Message = "some items good for sale";
                response_ShoppingCart.TotalPrice = totalPrice;
            }
            return response_ShoppingCart;

        }
        public async Task<Response_ShoppingCartInfo> AddItem(string userId, Request_ShoppingCart shoppingCartItem)
        {
            var userCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.ApiUserId == userId);
            if (userCart == null)
            {
                var shoppingCart = new ShoppingCart();
                shoppingCart.ApiUserId = userId;
                await _dbContext.ShoppingCarts.AddAsync(shoppingCart);
                await _dbContext.SaveChangesAsync();

            }
            var cartItem = new CartItem();
            cartItem.ProductVariantId = shoppingCartItem.ProductVariantId;
            var productVariant = await _dbContext.ProductVariants.FirstOrDefaultAsync(x => x.Id == shoppingCartItem.ProductVariantId);
            if (productVariant == null)
            {
                return new Response_ShoppingCartInfo()
                {
                    IsSuccess = false,
                    Message = "Theres no Product Variant available",
                    ProductVariantId = 0,
                    RequestQty = 0,
                    StoreQty = 0

                };
            }

            var cartItemInDb = await _dbContext.CartItems.AnyAsync(x => x.ShoppingCartId == userCart.Id && x.ProductVariantId == productVariant.Id);
            if (cartItemInDb == true)
            {
                return new Response_ShoppingCartInfo
                {
                    IsSuccess = false,
                    Message = "This item is already in your cart",
                    ProductVariantId = 0,
                    RequestQty = 0,
                    StoreQty = 0
                };
            }
            if (productVariant.Quantity < shoppingCartItem.Quantity)
            {
                return new Response_ShoppingCartInfo
                {
                    IsSuccess = false,
                    Message = "Not enough items in stock",
                    RequestQty = shoppingCartItem.Quantity,
                    StoreQty = productVariant.Quantity
                };
            }
            cartItem.Quantity = shoppingCartItem.Quantity;
            cartItem.ShoppingCartId = userCart.Id;
            await _dbContext.CartItems.AddAsync(cartItem);
            await _dbContext.SaveChangesAsync();

            return new Response_ShoppingCartInfo
            {
                IsSuccess = true,
                ProductVariantId = productVariant.Id,
                Message = "Product Added to cart",
                RequestQty = shoppingCartItem.Quantity,
                StoreQty = productVariant.Quantity
            };

        }

        public Task<Response_ShoppingCartInfo> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

       

        public Task<Response_ShoppingCartInfo> RemoveItem(string userId, Request_ShoppingCart shoppingCartItem)
        {
            throw new NotImplementedException();
        }

        public Task<Response_ShoppingCartInfo> UpdateQty(string userId, Request_ShoppingCart shoppingCartItem)
        {
            throw new NotImplementedException();
        }
    }
}
