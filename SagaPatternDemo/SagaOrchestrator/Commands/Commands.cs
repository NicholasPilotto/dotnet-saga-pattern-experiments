namespace SagaOrchestrator.Commands;

public record SendOrderPayment(Guid OrderId, decimal Price);

public record SendOrderShipment(Guid OrderId);