using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
	public class ResConsultarFeedback : ResBase
	{
		public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
	}
}
