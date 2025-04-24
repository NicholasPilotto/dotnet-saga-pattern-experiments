using MassTransit;
using MassTransit.NewIdProviders;
using SagaOrchestrator.Commands;
using SagaOrchestrator.Events;
using SagaOrchestrator.Persistence.Models;

namespace SagaOrchestrator.Saga;

public class OrderSagaStateMachine : MassTransitStateMachine<OrderSagaModel>
{
  public State Ordering { get; set; }
  public State Paying { get; set; }
  public State Shipping { get; set; }

  public Event<OrderCreated> OrderCreated { get; set; }
  public Event<OrderPayed> OrderPayed { get; set; }
  public Event<OrderShipped> OrderShipped { get; set; }

  public OrderSagaStateMachine()
  {
    InstanceState(x => x.CurrentState);

    Event(() => OrderCreated, e => e.CorrelateById(m => m.Message.OrderId));
    Event(() => OrderPayed, e => e.CorrelateById(m => m.Message.OrderId));
    Event(() => OrderShipped, e => e.CorrelateById(m => m.Message.OrderId));

    Initially(
      When(OrderCreated)
        .Then(context =>
        {
          context.Saga.OrderId = context.Message.OrderId;
          context.Saga.Price = context.Message.Price;
          context.Saga.OrderCreated = true;
        })
        .TransitionTo(Paying)
        .Publish(context => new SendOrderPayment(context.Message.OrderId, context.Message.Price)));

    During(Paying,
      When(OrderPayed)
        .Then(context =>
        {
          context.Saga.OrderCreated = true;
          context.Saga.OrderPayed = true;
        })
        .TransitionTo(Shipping)
        .Publish(context => new SendOrderShipment(context.Message.OrderId))
        .Finalize());
  }
}