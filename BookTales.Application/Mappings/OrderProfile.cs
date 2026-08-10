using AutoMapper;
using BookTales.Application.DTOs.Orders;
using BookTales.Domain.Entities;

namespace BookTales.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>();

            CreateMap<OrderItemDto, OrderItem>();

            CreateMap<Order, OrderDto>();

            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}