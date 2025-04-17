using MassTransit;

namespace OrderSaga.States;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public required string CurrentState { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
}