/*using System.Text.Json;

namespace Online_Library.ExtensionMethods
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, object value) where T : class
        {
            if (value == null)
            {
                session.Remove(key);
                return;
            } 
            else
            {
                string jsonData = JsonSerializer.Serialize(value);
                session.SetString(key, jsonData);
            }
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            if (!session.Keys.Contains(key))
            {
                return null;
            }
            
            string jsonData = session.GetString(key);

            if (String.IsNullOrEmpty(jsonData))
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }

    }

}
*/
