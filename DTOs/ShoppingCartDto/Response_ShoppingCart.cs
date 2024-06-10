namespace TestApi.DTOs.ShoppingCartDto
{
    public class Response_ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public bool CanBeSold { get; set; }
        public string Message { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Model_CartItemReturn> Items { get; set; } = new List<Model_CartItemReturn>();
        public List<Model_CartItemReturn> ItemsCantBeSold { get; set; } = new List<Model_CartItemReturn>();
    }
}
