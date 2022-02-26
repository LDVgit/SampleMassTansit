namespace SampleMassTransit.Components
{
    using System;

    using Automatonymous;

    public class OrderState :
        SagaStateMachineInstance
    {
        public string CurrentState { get; set; }

        /// Идентификатор эеземпляра машины состояния сага
        public Guid CorrelationId { get; set; }
    }
}