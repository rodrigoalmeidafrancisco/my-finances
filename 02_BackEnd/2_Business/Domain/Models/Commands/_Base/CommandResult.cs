using Flunt.Notifications;
using Shared.Usefuls;

namespace Domain.Models.Commands._Base
{
    public class CommandResult<T>
    {
        public CommandResult()
        {
            Data = Data.IsList() ? Activator.CreateInstance<T>() : default;
        }

        public int StatusCod { get; set; }
        public string Message { get; set; }
        public string ErroId { get; set; } = null;
        public List<string> Errors { get; set; } = null;
        public T Data { get; set; }

        public void ConvertStatus200(T data, string message = null)
        {
            StatusCod = 200;
            Message = message;
            Data = data;
        }

        public void ConvertStatus201(T data, string message = null)
        {
            StatusCod = 201;
            Message = message;
            Data = data;
        }

        public void ConvertStatus400(string message, List<string> listErrors, IReadOnlyCollection<Notification> listNotification)
        {
            StatusCod = 400;
            Message = message;

            if (listErrors != null)
            {
                Errors.AddRange(listErrors);
            }

            if (listNotification != null)
            {
                Errors.AddRange(listNotification.SelectFluntNotification());
            }
        }

        public void ConvertStatus500(string erroId, string message, List<string> listErrors = null)
        {
            StatusCod = 500;
            Message = message;
            ErroId = erroId;

            if (listErrors != null)
            {
                Errors.AddRange(listErrors);
            }
        }

    }
}
