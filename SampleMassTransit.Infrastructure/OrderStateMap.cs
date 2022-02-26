﻿namespace SampleMassTransit.Infrastructure
{
    using Components;

    using MassTransit.EntityFrameworkCoreIntegration.Mappings;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
        }
    }
}