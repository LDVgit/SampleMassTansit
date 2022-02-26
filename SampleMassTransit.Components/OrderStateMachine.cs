namespace SampleMassTransit.Components
{
    using Automatonymous;

    using Contracts;

    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, 
                x => 
                    x.CorrelateById(m => m.Message.OrderId));

            InstanceState(x => x.CurrentState);
            
            Initially(
                When(OrderSubmitted)
                    .TransitionTo(Submitted)
                );    
            
        }
        
        public State Submitted { get; private set; }
        
        public Event<IOrderSubmitted> OrderSubmitted { get; private set; }
    }
}