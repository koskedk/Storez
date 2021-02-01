using System.Threading.Tasks;
using MassTransit;
using Serilog;
using Storez.Contracts;

namespace Storez.Components.Consumers
{
    public class SubmitOrderConsumer:IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            Log.Debug($"Order submitted {nameof(SubmitOrder.CustomerNumber)}:{context.Message.CustomerNumber}");

            await context.RespondAsync<OrderSubmission>(new
            {
                context.Message.OrderId,
                InVar.Timestamp,
                context.Message.CustomerNumber
            });

            await context.Publish<OrderSubmitted>(new
            {
                context.Message.OrderId,
                context.Message.Timestamp,
                context.Message.CustomerNumber
            });
        }
    }
}
