using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogBLL.ViewModels
{
    public class ShowCommit
    //评论显示的viewmodels
    {
        public int Id { get; set; }//评论的ID号
        public int TextId { get; set; }//评论文章的ID
        public int TextTotalCommit { get; set; }//评论文章的评论数量
        public string TextTitle { get; set; }//评论文章的标题
        public int Num { get; set; }//第几楼
        public string Name { get; set; }//昵称
        public string Account { get; set; }//用户名
        public string Content { get; set; }//内容
        public string Date { get; set; }//发布时间
    }

    public class TextListsHot
    //文章热度显示
    {
        [Key]
        public int TextID { get; set; }//文章唯一标识

        [Required]
        [DisplayName("文章标题")]
        [MaxLength(40)]
        public string TextTitle { get; set; }//标题


        [DisplayName("点击量")]
        public int Hot { get; set; }//点击量


        public string CategoryName { get; set; }//分类

        public string Datemouth { get; set; }//月份（字符串）
    }


    public class CategoryList
    //分类显示
    {
        [MaxLength(12)]
        [Display(Name = "类别名称")]
        public string CategoryName { get; set; }
        [Display(Name = "文章数")]
        public int TextCount { get; set; }
        [Display(Name = "总热度")]
        public int CategoryHot { get; set; }
    }

    public class TextIndex
    {
        public int TextID { get; set; }//文章唯一标识

        [DisplayName("文章标题")]

        public string TextTitle { get; set; }//标题

        public string FirstView { get; set; }//摘要

        [DisplayName("文章正文")]
        public string Text { get; set; }//内容

        [DisplayName("热度")]
        public int Hot { get; set; }//热度

        [DisplayName("分类")]
        public string CategoryName { get; set; }//分类

        [DisplayName("更新时间")]
        public DateTime TextChangeDate { get; set; }//更新时间

        [DisplayName("评论数")]
        public int CommitCount { get; set; }//评论数量

    }


}
