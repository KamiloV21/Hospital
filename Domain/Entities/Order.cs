using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public int Doctor_Id { get; set; }
		public string Email { get; set; }
		public DateTime StartDate { get; set; }
		public virtual Doctor Doctor { get; set; }
	}
}
