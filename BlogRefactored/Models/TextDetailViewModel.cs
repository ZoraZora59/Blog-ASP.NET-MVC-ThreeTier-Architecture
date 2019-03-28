using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BlogRefactored.Models
{
	public class TextDetailViewModel
	{
		public int TextID { get; set; }//文章唯一标识
		
		[DisplayName("文章标题")]
		public string TextTitle { get; set; }//标题
		[DisplayName("文章正文")]
		public string Text { get; set; }//内容
		[DisplayName("点击量")]
		public int Hot { get; set; }//点击量
		[DisplayName("分类")]
		public string CategoryName { get; set; }//分类
		[DisplayName("更新时间")]
		public DateTime TextChangeDate { get; set; }//更新时间
	}
}