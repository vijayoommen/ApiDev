using AutoMapper;
using MySiteContracts.Sales;
using ShoppingSite.DataAccess;

namespace ShoppingSite;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
            .ReverseMap();

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();

        CreateMap<CartItemDto, CartToSalesOrder.CartItemForSalesOrderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

        CreateMap<CartDto, CartToSalesOrder>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
    }
}
