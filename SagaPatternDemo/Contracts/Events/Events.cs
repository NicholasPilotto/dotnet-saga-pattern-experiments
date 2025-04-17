namespace Contracts.Events;

public record OrderSubmitted(Guid OrderId, decimal Amount);
public record PaymentCompleted(Guid OrderId);
public record PaymentFailed(Guid OrderId, string Reason);
public record ItemShipped(Guid OrderId);
