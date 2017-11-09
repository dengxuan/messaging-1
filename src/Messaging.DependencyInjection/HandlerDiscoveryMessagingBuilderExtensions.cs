﻿using System;
using Messaging;
using Messaging.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HandlerDiscoveryMessagingBuilderExtensions
    {
        public static MessagingBuilder AddHandlerDiscovery(this MessagingBuilder builder, Action<DiscoverySettings> action)
        {
            var discoverySettings = new DiscoverySettings();

            action(discoverySettings);

            builder.AddHandlerDiscovery(discoverySettings);

            return builder;
        }

        public static MessagingBuilder AddHandlerDiscovery(this MessagingBuilder builder)
        {
            var discoverySettings = new DiscoverySettings();

            builder.AddHandlerDiscovery(discoverySettings);

            return builder;
        }

        internal static MessagingBuilder AddHandlerDiscovery(this MessagingBuilder builder, DiscoverySettings discoverySettings)
        {
            builder.Services.Scan(s =>
                s.FromAssemblies(discoverySettings.MessageHandlerAssemblies)
                    .AddClasses(f => f.AssignableTo(typeof(IMessageHandler<>)), !discoverySettings.IncludeNonPublic)
                    .UsingRegistrationStrategy(discoverySettings.RegistrationStrategy)
                    .AsSelf()
                    .WithLifetime(discoverySettings.DiscoveredHandlersLifetime));

            return builder;
        }

    }
}