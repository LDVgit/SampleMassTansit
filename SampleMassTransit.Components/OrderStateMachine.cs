namespace SampleMassTransit.Components
{
    using System;

    using Automatonymous;

    using Contracts;

    using MassTransit;

    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }

        public Event<IOrderSubmitted> OrderSubmitted { get; private set; }
        public Event<ICheckOrder> OrderStatusSubmitted { get; private set; }

        public OrderStateMachine()
        {
            Event(() => OrderSubmitted,
                x =>
                    x.CorrelateById(m => m.Message.OrderId));

            Event(() => OrderStatusSubmitted,
                x =>
                    x.CorrelateById(m => m.Message.OrderId)
                        .OnMissingInstance(m =>
                            m.ExecuteAsync(async context =>
                            {
                                if (context.RequestId.HasValue)
                                    await context.RespondAsync<IOrderNotFound>(new
                                    {
                                        context.Message.OrderId
                                    });
                            })
                        )
            );


            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.Updated = context.Data.TimeStamp;
                        context.Instance.SubmitDate = DateTime.UtcNow;
                        context.Instance.CustomerNumber = context.Data.CustomerNumber;
                    })
                    .TransitionTo(Submitted)
            );

            During(Submitted,
                Ignore(OrderSubmitted));

            DuringAny(
                When(OrderStatusSubmitted)
                    .RespondAsync(x => x.Init<IOrderStatus>(
                        new
                        {
                            OrderId = x.Instance.CorrelationId,
                            State = x.Instance.CurrentState
                        })
                    )
            );

            DuringAny(When(OrderSubmitted)
                .Then(context =>
                {
                    context.Instance.SubmitDate ??= DateTime.UtcNow;
                    context.Instance.CustomerNumber ??= context.Data.CustomerNumber;
                    context.Instance.Updated ??= DateTime.UtcNow;
                }));
        }
    }
}