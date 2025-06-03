using FUNewsManagement.DataAccess.Models;
using System.Text.Json;

namespace StudentNameMVC.Helpers
{
    public static class SessionHelper
    {
        private const string USER_SESSION_KEY = "CurrentUser";

        public static void SetCurrentUser(ISession session, SystemAccount user)
        {
            var userJson = JsonSerializer.Serialize(user);
            session.SetString(USER_SESSION_KEY, userJson);
        }

        public static SystemAccount? GetCurrentUser(ISession session)
        {
            var userJson = session.GetString(USER_SESSION_KEY);
            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonSerializer.Deserialize<SystemAccount>(userJson);
        }

        public static void ClearCurrentUser(ISession session)
        {
            session.Remove(USER_SESSION_KEY);
        }

        public static bool IsLoggedIn(ISession session)
        {
            return GetCurrentUser(session) != null;
        }

        public static bool IsAdmin(ISession session, IConfiguration configuration)
        {
            var user = GetCurrentUser(session);
            if (user == null) return false;

            var adminEmail = configuration["AdminAccount:Email"];
            return user.AccountEmail == adminEmail;
        }

        public static bool IsStaff(ISession session)
        {
            var user = GetCurrentUser(session);
            return user?.AccountRole == 1;
        }

        public static bool IsLecturer(ISession session)
        {
            var user = GetCurrentUser(session);
            return user?.AccountRole == 2;
        }
    }
}
