using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogRefactored.Models
{
	public class TextsListViewModel
	{
		public List<TextDetailViewModel> Texts { get; set; }
		public int TextsCount { get; set; }
		public int Pages { get; set; }
		public int PageCount { get; set; }
	}
}