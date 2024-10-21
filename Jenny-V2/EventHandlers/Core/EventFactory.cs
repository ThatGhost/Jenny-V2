using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Jenny_V2.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Jenny_V2.EventHandlers.Core
{
    public class EventFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EventFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void HandleEvent(TextCommand eventType, string text)
        {
            // Find the type that has the EventHandlerAttribute for the given eventType
            var handlerType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IEventHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .FirstOrDefault(t => t.GetCustomAttributes(typeof(EventHandlerAttribute), false)
                .Cast<EventHandlerAttribute>()
                .Any(attr => attr.EventType == eventType));

            if (handlerType == null)
                throw new InvalidOperationException($"Handler for {eventType} not found.");

            IEventHandler handler = (IEventHandler)_serviceProvider.GetRequiredService(handlerType);
            handler.Handle(text);
        }
    }
}
