namespace Hydra.Order.API.Models
{
    public class NotificationsDto
    {
        public string Key { get; private set; }
        public string Message { get; private set; }

        public NotificationType NotificationType { get; private set; }

        protected NotificationsDto(){ }

        public NotificationsDto AddModelError(string key, string errorMessage)
        {
            Key = key;
            Message = errorMessage;
            NotificationType = NotificationType.Error;

            return this;
        }
    }
     public enum NotificationType
    {
        Error = -1,
        Other = 0
    }
}