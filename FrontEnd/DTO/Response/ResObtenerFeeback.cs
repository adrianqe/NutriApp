using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriApp.DTO.Response
{
    public class ResObtenerFeeback : ResBase
    {
        public List<Feedback> feedback { get; set; }
    }
}
