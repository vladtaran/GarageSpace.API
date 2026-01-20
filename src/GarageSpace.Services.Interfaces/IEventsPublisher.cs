namespace GarageSpace.Services.Interfaces
{
    public interface IEventsPublisher
    {
        void Publish<TEvent>(TEvent message) where TEvent : IEventMessage;
    }
}