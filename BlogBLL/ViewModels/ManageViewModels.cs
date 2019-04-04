using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogBLL.ViewModels
{
	public class ManageMain//主界面信息
	{
		[DisplayName("当前用户数量")]
		public int UserCount { get; set; }
		[DisplayName("当前文章数量")]
		public int TextCount { get; set; }
		[DisplayName("当前评论数量")]
		public int CommentCount { get; set; }
		[DisplayName("当前总点击量")]
		public int HotCount { get; set; }
	}
	public class ManageText//文章管理列表
	{
		[DisplayName("索引号")]
		public int TextID { get; set; }
		[DisplayName("文章标题")]
		public string TextTitle { get; set; }
		[DisplayName("分类")]
        
        public string CategoryName { get; set; }
		[DisplayName("热度")]
		public int Hot { get; set; }
		[DisplayName("修改时间")]
		public string TextChangeDate { get; set; }
	}
	public class ManageUser//用户管理列表
	{
		[DisplayName("用户名")]
		public string Account { get; set; }
		[DisplayName("昵称")]
		public string Name { get; set; }
		[DisplayName("评论数量")]
		public int CommmentCount { get; set; }

	}
	public class ManageCategory//分类管理信息
	{
		[DisplayName("类别名称")]
		public string CategoryName { get; set; }
		[DisplayName("文章数")]
		public int TextCount { get; set; }
		[DisplayName("总热度")]
		public int CategoryHot { get; set; }
	}
	public class ManageComment//评论管理信息
	{
		public int Id { get; set; }//评论的ID号
		public int TextId { get; set; }//评论文章的ID
		public int TextCommentCount { get; set; }//评论文章的评论数量
		public string TextTitle { get; set; }//评论文章的标题
		public int Num { get; set; }//第几楼
		public string Name { get; set; }//昵称
		public string Account { get; set; }//用户名
		public string Content { get; set; }//内容
		public string Date { get; set; }//发布时间
	}

	public class BlogConfig//博客配置文件
	{
		[Required]
		[StringLength(maximumLength: 50)]
		public string Name { set; get; }

		[Required]
		[StringLength(maximumLength: 200)]
		public string Sign { set; get; }

		[Required]
		[StringLength(maximumLength: 500)]
		public string Note { set; get; }
	}
	public class UpdateText//更新博文\编辑博文时获取数据
	{
		[DisplayName("文章索引号")]
		public int Id { get; set; }

		[DisplayName("文章标题")]
        [Required]
		public string Title { get; set; }
		
		[DisplayName("文章内容")]
		public string Text { get; set; }

		[DisplayName("分类")]
        [RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]+$", ErrorMessage = "分类名必须是由中文、字母、数字、下划线的组合")]
        public string Category { get; set; }

	}
	public class DetailCategory//分类详情
	{
		public ManageCategory Category { get; set; }//分类属性
		public List<TextsBelone> Texts { get; set; }//从属文章
	}
	public class TextsBelone//分类从属下的文章列表
	{
		public int TextID { get; set; }//文章ID
		public string TextTitle { get; set; }//文章标题
		public int Hot { get; set; }//文章热度
		public string ChangeTime { get; set; }//修改时间
	}
}
