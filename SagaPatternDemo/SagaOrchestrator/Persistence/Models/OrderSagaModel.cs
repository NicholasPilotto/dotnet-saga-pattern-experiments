using MassTransit;

namespace SagaOrchestrator.Persistence.Models;

public class OrderSagaModel : SagaStateMachineInstance
{
  public Guid CorrelationId { get; set; }
  public required string CurrentState { get; set; }
  public Guid OrderId { get; set; }
  public decimal Price { get; set; }
  public bool OrderCreated { get; set; }
  public bool OrderPayed { get; set; }
  public bool OrderShipped { get; set; }
}