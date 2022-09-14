namespace EventBusPractice.Notifications.API.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        public User UserDetails { get; set; }
    }
}
