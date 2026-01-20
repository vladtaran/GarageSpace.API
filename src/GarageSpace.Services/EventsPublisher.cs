using GarageSpace.Services.Interfaces;

namespace GarageSpace.Services
{
    public class EventsPublisher : IEventsPublisher
    {
        public void Publish<TEvent>(TEvent message) where TEvent : IEventMessage
        {
            throw new NotImplementedException();
        }
    }
}
