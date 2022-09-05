using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities
{
	public class Position
	{
		[Key]
		public int Id { get; set; }
		public string Role { get; set; }
		public double Price { get; set; }
		public virtual ICollection<Doctor> Doctors { get; set; }
	}
}
