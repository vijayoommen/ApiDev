using AutoMapper;
using SalesOrders.Models.Responses;

namespace SalesOrders;

public class SalesOrderProfile : Profile
{
    public SalesOrderProfile()
    {
        CreateMap<MySiteContracts.Sales.CartToSalesOrder, OrderDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id to allow new orders to be created
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.CartItems))
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
            .ReverseMap();

        CreateMap<MySiteContracts.Sales.CartToSalesOrder.CartItemForSalesOrderDto, OrderItemDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id to allow new items to be added
            .ReverseMap();

        CreateMap<DataAccess.Entities.Order, Models.Responses.OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ReverseMap();

        CreateMap<DataAccess.Entities.OrderItem, Models.Responses.OrderItemDto>()
            .ReverseMap();
    }
}
