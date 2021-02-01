using System;
using Automatonymous;
using MassTransit;
using Storez.Contracts;

namespace Storez.Components.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }
        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<CheckOrder> OrderStatusRequested { get;private set; }

        public OrderStateMachine()
        {
            Event(() => OrderSubmitted,
                x =>
                    x.CorrelateById(m => m.Message.OrderId));

            Event(() => OrderStatusRequested,
                x =>
                {
                    x.CorrelateById(m => m.Message.OrderId);
                    x.OnMissingInstance(m => m.ExecuteAsync(async c =>
                    {
                        if (c.RequestId.HasValue)
                        {
                            await c.RespondAsync<OrderNotFound>(new
                            {
                                c.Message.OrderId
                            });
                        }
                    }));
                });

            InstanceState(x=>x.CurrentState);

            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate = context.Data.Timestamp;
                        context.Instance.CustomerNumber = context.Data.CustomerNumber;
                        context.Instance.Updated=DateTime.UtcNow;
                    })
                    .TransitionTo(Submitted));

            During(Submitted,Ignore(OrderSubmitted));

            DuringAny(
                When(OrderSubmitted)
                    .Then(c =>
                    {
                        c.Instance.SubmitDate ??= c.Data.Timestamp;
                        c.Instance.CustomerNumber ??= c.Data.CustomerNumber;
                    }));

            DuringAny(When(OrderStatusRequested).RespondAsync(x=>
                x.Init<OrderStatus>(new
                {
                    OrderId=x.Instance.CorrelationId,
                    State=x.Instance.CurrentState,
                    x.Instance.CustomerNumber
                })));
        }
    }
}
