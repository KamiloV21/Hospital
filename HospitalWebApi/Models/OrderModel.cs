using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalWebApi.Models
{
	public class OrderModel
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public int Doctor_Id { get; set; }
		public string Email { get; set; }
		public DateTime StartDate { get; set; }
	}
}
