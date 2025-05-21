using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO
{
    public class ReqInsertarAlergias
    {
        public int userID { get; set; }
        public List<string> alergias { get; set; }
    }
}