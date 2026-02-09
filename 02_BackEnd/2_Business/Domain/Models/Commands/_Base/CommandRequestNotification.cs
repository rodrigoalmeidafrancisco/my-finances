using Flunt.Notifications;

namespace Domain.Models.Commands._Base
{
    public abstract class CommandRequestNotification : Notifiable<Notification>
    {
        public abstract void ValidateCommand();
    }
}
