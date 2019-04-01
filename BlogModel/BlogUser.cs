using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogModel
{
	public class BlogUser
	{
		[Key]
		[MaxLength(16)]
		public string Account { get; set; }

		[Required]
		[MaxLength(64)]
		public string Password { get; set; }

		[Required]
		[MaxLength(64)]
		public string Name { get; set; }

		public virtual ICollection<BlogComment> Commits { get; set; }
	}
}
