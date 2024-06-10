using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TestApi.Data.CustomModels;
using TestApi.DTOs.BasePorductDto.CustomModels;
using TestApi.DTOs.ShoppingCartDto;
using TestApi.Models.ProductModels;
using TestApi.Models.ShoppingCartModels;

namespace TestApi.DtoConvertations
{
    public static class ConvertToDto
    {
        public static IEnumerable<Model_BaseProductCustom> ConvertToDtoListCustomProduct(this IEnumerable<BaseProduct> baseProducts )
        {
            var baseProductCustomReturn = new List<Model_BaseProductCustom>();
            foreach(var baseProduct in baseProducts)
            {
                List<Model_BaseImageCustom> images = new List<Model_BaseImageCustom>();
                foreach(var imageBase in baseProduct.imageBases)
                {
                    var baseImageCustom = new Model_BaseImageCustom()
                    {
                        Id = imageBase.Id,
                        BaseProductId = imageBase.BaseProductId,
                        AddedOn = imageBase.AddedOn,
                        staticPath = imageBase.StaticPath
                    };
                }
                List<Model_ProductVariantCustom> productVariants = new List<Model_ProductVariantCustom>();
                foreach(var productVariant in baseProduct.productVariants)
                {
                    var variantColor = new Model_ProductColorCustom()
                    {
                        Id = productVariant.ProductColor.Id,
                        Name = productVariant.ProductColor.Name
                    };
                    var variantSize = new Model_ProductSizeCustom()
                    {
                        Id = productVariant.ProductSize.Id,
                        Name = productVariant.ProductSize.Name
                    };
                    var prodcutVariantCustom = new Model_ProductVariantCustom()
                    {
                        Id = productVariant.Id,
                        BaseProductId = productVariant.BaseProductId,
                        productColor = variantColor,
                        productSize = variantSize,
                        Quantity = productVariant.Quantity
                    };
                    productVariants.Add(prodcutVariantCustom);
                }
                var baseProductCustom = new Model_BaseProductCustom()
                {
                    Id = baseProduct.Id,
                    Description = baseProduct.Description,
                    Discount = baseProduct.Discount,
                    ImageBase = images,
                    MainCategory = baseProduct.MainCategory,
                    Material = baseProduct.Material,
                    Name = baseProduct.Name,
                    Price = baseProduct.Price,
                    ProductVariant = productVariants,
                    TotalPrice = baseProduct.TotalPrice,
                };
                baseProductCustomReturn.Add(baseProductCustom);
            }
            return baseProductCustomReturn;

        } 

        public static Model_BaseProductCustom ConvertToDtoCustomProduct(this BaseProduct baseProduct)
        {
            var baseProductCustom = new Model_BaseProductCustom();

            List<Model_BaseImageCustom> images = new List<Model_BaseImageCustom>();
            foreach(var image in baseProduct.imageBases)
            {
                var imageBaseCustom = new Model_BaseImageCustom()
                {
                    Id= image.Id,
                    AddedOn = image.AddedOn,
                    BaseProductId = image.BaseProductId,
                    staticPath = image.StaticPath
                };
                images.Add(imageBaseCustom);
            } 

            List<Model_ProductVariantCustom> productVariantCustoms = new List<Model_ProductVariantCustom>();
            foreach(var productVariant in baseProduct.productVariants)
            {
                var productColor = new Model_ProductColorCustom()
                {
                    Id = productVariant.ProductColor.Id,
                    Name = productVariant.ProductColor.Name
                };

                var productSize = new Model_ProductSizeCustom()
                {
                    Id = productVariant.ProductSize.Id,
                    Name = productVariant.ProductSize.Name
                };

                var productVariantCustom = new Model_ProductVariantCustom()
                {
                    Id = productVariant.Id,
                    BaseProductId = productVariant.BaseProductId,
                    productColor = productColor,
                    productSize = productSize,
                    Quantity = productVariant.Quantity
                };
                productVariantCustoms.Add(productVariantCustom);
            }
            baseProductCustom.Id = baseProduct.Id;
            baseProductCustom.Description = baseProduct.Description;
            baseProductCustom.Discount = baseProduct.Discount;
            baseProductCustom.ImageBase = images;
            baseProductCustom.MainCategory = baseProduct.MainCategory;
            baseProductCustom.Material = baseProduct.Material;
            baseProductCustom.Name = baseProduct.Name;
            baseProductCustom.Price = baseProduct.Price;
            baseProductCustom.ProductVariant = productVariantCustoms;

            return baseProductCustom;
        }

        public static Model_BaseProductWithNoExtraInfo ConvertToDtoProductNoInfo(this BaseProduct baseProduct)
        {
            return new Model_BaseProductWithNoExtraInfo
            {
                Id = baseProduct.Id,
                Description = baseProduct.Description,
                Discount = baseProduct.Discount,
                MainCategoryId = baseProduct.MainCategoryId,
                MaterialId = baseProduct.MaterialId,
                Name = baseProduct.Name,
                Price = baseProduct.Price,
                TotalPrice = baseProduct.TotalPrice
            };
        }

        public static Model_CartItemReturn ConvertToDtoCartItem(this BaseProduct baseProduct , ProductVariant productVariant , CartItem cartItem)
        {
            Model_CartItemReturn cartItemReturn = new Model_CartItemReturn();
            bool enoughItems = true;

            if(productVariant.Quantity < cartItem.Quantity)
            {
                enoughItems = false;
            }
            cartItemReturn.BaseProductId = baseProduct.Id;
            cartItemReturn.BaseProductName = baseProduct.Name;
            cartItemReturn.BaseProductDescription = baseProduct.Description;
            cartItemReturn.Price = baseProduct.Price;
            cartItemReturn.Discount = baseProduct.Discount;
            cartItemReturn.TotalPrice = baseProduct.TotalPrice;
            cartItemReturn.ProductVariantId = productVariant.Id;

            Model_ProductColorCustom productColorCustom = new Model_ProductColorCustom()
            {
                Id = productVariant.ProductColor.Id,
                Name = productVariant.ProductColor.Name,
            };

            Model_ProductSizeCustom productSizeCustom = new Model_ProductSizeCustom()
            {
                Id = productVariant.ProductSize.Id,
                Name = productVariant.ProductSize.Name,
            };
            cartItemReturn.ProductColor = productColorCustom;
            cartItemReturn.ProductSize = productSizeCustom;
            cartItemReturn.AvailableQuantity = productVariant.Quantity;
            cartItemReturn.SelectedQuantity = cartItem.Quantity;
            cartItemReturn.CanBeSold = enoughItems;

            cartItemReturn.TotalPrice = cartItem.Quantity * baseProduct.TotalPrice;

            if(enoughItems)
            {
                cartItemReturn.Message = "Item can be sold";
            }
            else
            {
                cartItemReturn.Message = "Not enough items";
            }
            return cartItemReturn;
        }

        
    }
}