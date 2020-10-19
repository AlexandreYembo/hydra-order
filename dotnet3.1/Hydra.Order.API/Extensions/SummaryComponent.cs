using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Models;
using MediatR;

namespace Hydra.Order.API.Extensions
{
    public class SummaryComponent : NotificationsDto
    {
        public List<NotificationsDto> notificationsDto {get;set;}
        private readonly DomainNotificationHandler _notifications;
        public SummaryComponent(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
            notificationsDto = new List<NotificationsDto>();
        }   

        /// <summary>
        /// TODO: Implement the notification you want to send to the user, you can use SignalR to return the messages
        /// </summary>
        /// <returns></returns>
        public async Task<List<NotificationsDto>> InvokeAsync()
        {
            var notifications = await Task.FromResult(_notifications.GetNotifications());
            notifications.ForEach(c => notificationsDto.Add(AddModelError(c.Key, c.Value)));
            
            return notificationsDto;
        }
    }
}