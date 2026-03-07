using Flunt.Notifications;
using System.Collections;

namespace Shared.Usefuls
{
    public static class UsefulExtension
    {
        public static bool IsList<T>(this T obj)
        {
            if (obj == null)
                return false;

            var type = typeof(T);
            return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static List<string> SelectFluntNotification(this IReadOnlyCollection<Notification> listNotification)
        {
            if (listNotification == null || listNotification.Count == 0)
                return [];

            return [.. listNotification.Select(n => n.Message).Where(m => !string.IsNullOrWhiteSpace(m)).Distinct(StringComparer.OrdinalIgnoreCase)];
        }
    }
}
