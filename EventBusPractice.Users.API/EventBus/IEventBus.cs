namespace EventBusPractice.Users.API.EventBus
{
    public interface IEventBus
    {
        void Publish(string integrationEvent, string eventData);
    }
}
