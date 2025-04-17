using Contracts.Events;

namespace PaymentService.Consumers;
using MassTransit;

public class OrderSubmittedConsumer : IConsumer<OrderSubmitted>
{
    public async Task Consume(ConsumeContext<OrderSubmitted> context)
    {
        // Simulate payment logic
        if (context.Message.Amount < 1000)
        {
            await context.Publish(new PaymentCompleted(context.Message.OrderId));
        }
        else
        {
            await context.Publish(new PaymentFailed(context.Message.OrderId, "Amount too high"));
        }
    }
}