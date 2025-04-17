using Contracts.Events;
using MassTransit;
using OrderSaga.States;

namespace OrderSaga.StateMachine;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State Submitted { get; private set; }
    public State PaymentCompletedState { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    public Event<OrderSubmitted> OrderSubmittedEvent { get; private set; }
    public Event<PaymentCompleted> PaymentCompletedEvent { get; private set; }
    public Event<PaymentFailed> PaymentFailedEvent { get; private set; }
    public Event<ItemShipped> ItemShippedEvent { get; private set; }

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderSubmittedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => PaymentFailedEvent, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => ItemShippedEvent, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
            When(OrderSubmittedEvent)
                .Then(context => context.Saga.Amount = context.Message.Amount)
                .TransitionTo(Submitted)
                .Publish(context => new OrderSubmitted(context.Saga.CorrelationId, context.Saga.Amount))
        );

        During(Submitted,
            When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompletedState)
                .Publish(context => new PaymentCompleted(context.Saga.CorrelationId)),

            When(PaymentFailedEvent)
                .Then(context => context.Saga.Reason = context.Message.Reason)
                .TransitionTo(Failed)
        );

        During(PaymentCompletedState,
            When(ItemShippedEvent)
                .TransitionTo(Completed)
        );
    }
}
