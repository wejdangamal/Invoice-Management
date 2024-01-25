using AutoMapper;
using SalesInvoice.Models;
using SalesInvoice.Models.ViewModels;

namespace SalesInvoice.AutoMapper
{
    public class Mapp:Profile
    {
        public Mapp()
        {
            CreateMap<Category, CategoryVM>()
                .ReverseMap();
            CreateMap<Product, ProductVM>()
                .ReverseMap();
            CreateMap<Product, Product>()
                .ReverseMap();
            CreateMap<OrderMasterDetailsVM, Order>()
                .ForMember(dest=>dest.order_code,src=>src.MapFrom(src=>src.orderCode))
                .ReverseMap();
            CreateMap<listOfProducts, OrderItems>()
                .ForMember(dest=>dest.productId,src=>src.MapFrom(src=>src.product_Id))
                .ForMember(dest=>dest.Price,src=>src.MapFrom(src=>src.price))
                .ForMember(dest=>dest.QuantityOfProduct,src=>src.MapFrom(src=>src.quantity))
                .ReverseMap();
            CreateMap<listOfProducts, Product>()
                .ForMember(dest => dest.productName, src => src.MapFrom(src => src.product_Name))
                .ReverseMap();
        }
    }
}
