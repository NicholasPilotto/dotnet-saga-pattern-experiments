namespace OrderService.Commands;

public record OrderCreated(Guid OrderId, decimal Price);