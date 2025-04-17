using Contracts.Events;
using MassTransit;

namespace ShippingService.Consumers;

public class PaymentCompletedConsumer : IConsumer<PaymentCompleted>
{
    public async Task Consume(ConsumeContext<PaymentCompleted> context)
    {
        await Task.Delay(500); // Simulate shipping
        await context.Publish(new ItemShipped(context.Message.OrderId));
    }
}