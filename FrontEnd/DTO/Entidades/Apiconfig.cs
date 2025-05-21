using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace NutriApp.DTO
{
    public class ApiConfig
    {
        // Para cambiar el URL base de ngrok
        public static string UrlBase { get; set; } = "https://22f7-200-105-99-95.ngrok-free.app/";
    }
}
