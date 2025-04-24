namespace SagaOrchestrator.Events;

public record OrderCreated(Guid OrderId, decimal Price);

public record OrderPayed(Guid OrderId, Guid PaymentId, decimal Price);

public record OrderShipped(Guid OrderId, Guid PaymentId, Guid ShipmentId);