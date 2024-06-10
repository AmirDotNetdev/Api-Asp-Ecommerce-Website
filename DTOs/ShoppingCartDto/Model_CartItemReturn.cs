using TestApi.Data.CustomModels;

namespace TestApi.DTOs.ShoppingCartDto
{
    public class Model_CartItemReturn
    {
        public int BaseProductId { get; set; }
        public string BaseProductName { get; set; }
        public string BaseProductDescription { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal TotalPricePerItem { get; set; }
        public int ProductVariantId { get; set; }
        public Model_ProductColorCustom ProductColor { get; set; }
        public Model_ProductSizeCustom ProductSize { get; set; }
        public int AvailableQuantity { get; set; }
        public int SelectedQuantity { get; set; }
        public bool CanBeSold { get; set; }
        public decimal TotalPrice { get; set; }
        public string Message { get; set; }
    }
}
