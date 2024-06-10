using System.Text.Json.Serialization;

namespace TestApi.Models.ShoppingCartModels
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int ShoppingCartId { get; set; }
        [JsonIgnore]
        public virtual ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
