namespace TestApi.DTOs.ShoppingCartDto
{
    public class Response_ShoppingCartInfo
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int ProductVariantId { get; set; }
        public int RequestQty { get; set; }
        public int StoreQty { get; set; }
    }
}
