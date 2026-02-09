using System.Text.Json.Serialization;

namespace Domain.Models.Commands._Base
{
    public class CommandRequest : CommandRequestNotification
    {
        public CommandRequest()
        {
        }

        [JsonIgnore]
        public string UserLog { get; set; }
        [JsonIgnore]
        public List<string> UserRoles { get; set; }

        public override void ValidateCommand()
        {
            if (string.IsNullOrWhiteSpace(UserLog))
            {
                AddNotification(nameof(UserLog), "Usuário Log é obrigatório.");
            }

            if (UserLog == null || (UserLog != null && UserLog.Any()) == false)
            {
                AddNotification(nameof(UserLog), "Usuário Roles é obrigatório.");
            }
        }
    }
}
