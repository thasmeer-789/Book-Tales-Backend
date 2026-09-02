using AutoMapper;
using BookTales.Application.DTOs.Orders;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;
using BookTales.Domain.Enums;

namespace BookTales.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IBookRepository bookRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
    {
        if (dto.OrderItems == null || !dto.OrderItems.Any())
        {
            throw new InvalidOperationException(
                "Order must contain at least one item.");
        }

        if (dto.OrderItems.Any(item => item.Quantity <= 0))
        {
            throw new InvalidOperationException(
                "Order item quantity must be greater than zero.");
        }

        var bookIds = dto.OrderItems
            .Select(item => item.BookId)
            .Distinct()
            .ToList();

        var books = await _bookRepository.GetByIdsAsync(bookIds);

        var booksById = books.ToDictionary(book => book.Id);

        var order = new Order
        {
            UserId = dto.UserId,
            OrderDate = DateTime.UtcNow
        };

        decimal totalAmount = 0;

        foreach (var itemDto in dto.OrderItems)
        {
            if (!booksById.TryGetValue(itemDto.BookId, out var book))
            {
                throw new KeyNotFoundException(
                    $"Book with ID {itemDto.BookId} was not found.");
            }

            if (book.Stock < itemDto.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for book '{book.Title}'.");
            }

            var orderItem = new OrderItem
            {
                BookId = book.Id,
                BookTitle = book.Title,
                Quantity = itemDto.Quantity,
                Price = book.Price
            };

            order.OrderItems.Add(orderItem);

            totalAmount += book.Price * itemDto.Quantity;
        }

        order.TotalAmount = totalAmount;

        var createdOrder = await _orderRepository.CreateAsync(order);

        return _mapper.Map<OrderDto>(createdOrder);
    }

    public async Task<IEnumerable<OrderDto>> GetMyOrdersAsync(Guid userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(
        Guid orderId,
        Guid userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return null;

        if (order.UserId != userId)
            return null;

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto?> UpdateOrderStatusAsync(
        Guid orderId,
        UpdateOrderStatusDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return null;

        // An order whose payment hasn't succeeded can only be
        // Cancelled — it can never be Confirmed/Shipped/Delivered,
        // regardless of what OrderStatus transitions would otherwise
        // be allowed below.
        if (dto.Status != OrderStatus.Cancelled &&
            order.PaymentStatus != PaymentStatus.Paid)
        {
            throw new InvalidOperationException(
                "Cannot progress an order that has not been paid for.");
        }

        var isValidTransition = order.Status switch
        {
            OrderStatus.Pending =>
                dto.Status == OrderStatus.Confirmed ||
                dto.Status == OrderStatus.Cancelled,

            OrderStatus.Confirmed =>
                dto.Status == OrderStatus.Shipped ||
                dto.Status == OrderStatus.Cancelled,

            OrderStatus.Shipped =>
                dto.Status == OrderStatus.Delivered,

            OrderStatus.Delivered => false,

            OrderStatus.Cancelled => false,

            _ => false
        };

        if (!isValidTransition)
        {
            throw new InvalidOperationException(
                $"Cannot change order status from {order.Status} to {dto.Status}.");
        }

        order.Status = dto.Status;

        await _orderRepository.UpdateAsync(order);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto?> UpdatePaymentStatusAsync(
        Guid orderId,
        UpdatePaymentStatusDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return null;

        var isValidTransition = order.PaymentStatus switch
        {
            PaymentStatus.Pending =>
                dto.PaymentStatus == PaymentStatus.Paid ||
                dto.PaymentStatus == PaymentStatus.Failed,

            PaymentStatus.Paid =>
                dto.PaymentStatus == PaymentStatus.Refunded,

            PaymentStatus.Failed => false,

            PaymentStatus.Refunded => false,

            _ => false
        };

        if (!isValidTransition)
        {
            throw new InvalidOperationException(
                $"Cannot change payment status from {order.PaymentStatus} to {dto.PaymentStatus}.");
        }

        order.PaymentStatus = dto.PaymentStatus;

        await _orderRepository.UpdateAsync(order);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> CancelOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return null;

        if (order.Status != OrderStatus.Pending &&
            order.Status != OrderStatus.Confirmed)
        {
            throw new InvalidOperationException(
                $"Cannot cancel an order with status {order.Status}.");
        }

        order.Status = OrderStatus.Cancelled;

        await _orderRepository.UpdateAsync(order);

        return _mapper.Map<OrderDto>(order);
    }
}