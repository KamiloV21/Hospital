using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities
{
	public class Doctor
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public int Years { get; set; }
		public int Position_Id { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
		public virtual Position Position { get; set; }
	}
}
