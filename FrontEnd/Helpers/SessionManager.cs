using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp
{
    // Clase para manejar tokens y sesión
    public static class SessionManager
    {
        public static string AccessToken { get; set; }
        public static int UserId { get; set; }
        public static bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);

        public static void SaveSession(string token, int userId)
        {
            AccessToken = token;
            UserId = userId;
            // Opcional: Guardar en almacenamiento seguro
            Preferences.Set("access_token", token);
            Preferences.Set("user_id", userId);
        }

        public static void ClearSession()
        {
            AccessToken = null;
            UserId = 0;
            Preferences.Remove("access_token");
            Preferences.Remove("user_id");
        }

        public static void LoadSavedSession()
        {
            AccessToken = Preferences.Get("access_token", null);
            UserId = Preferences.Get("user_id", 0);
        }
    }
}
