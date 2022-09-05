using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalWebApi.Models
{
	public class DoctorModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Years { get; set; }
		public int Position_Id { get; set; }
	}
}
