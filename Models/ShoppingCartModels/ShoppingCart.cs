using System.Text.Json.Serialization;
using TestApi.Models.AuthModels;

namespace TestApi.Models.ShoppingCartModels
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public String ApiUserId { get; set; }
        [JsonIgnore]
        public virtual ApiUser ApiUser{ get; set; }
        public virtual ICollection<CartItem> cartItems { get; set; }
    }
}
