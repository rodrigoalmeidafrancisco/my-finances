using Flunt.Notifications;

namespace Shared.Usefuls
{
    public static class UsefulExtension
    {
        extension<T>(T obj)
        {
            public bool IsList()
            {
                if (typeof(T).IsGenericType)
                {
                    if (typeof(T).GetGenericTypeDefinition() == typeof(IList<>) ||
                        typeof(T).GetGenericTypeDefinition() == typeof(List<>) ||
                        typeof(T).GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        extension(IReadOnlyCollection<Notification> listNotification)
        {
            public List<string> SelectFluntNotification()
            {
                var listResult = new List<string>();

                if (listNotification != null && listNotification.Any())
                {
                    foreach (Notification notification in listNotification)
                    {
                        if (listResult.Any(x => x.Equals(notification.Message, StringComparison.OrdinalIgnoreCase)) == false)
                        {
                            listResult.Add(notification.Message);
                        }
                    }
                }

                return listResult.Distinct().ToList();
            }
        }



    }
}
