using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogBussinessLogic;
using BlogRefactored.Models;

namespace BlogRefactored.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";
			//TODO:设置每页显示的文章数量
			int ViewCount = 3;
			var manager = new BlogManager();
			var Texts = manager.GetAllTexts().Select(Text => new TextDetailViewModel()
			{
				TextID = Text.TextID,
				TextTitle = Text.TextTitle,
				TextChangeDate=Text.TextChangeDate,
				CategoryName=Text.CategoryName,
				Hot=Text.Hot,
				Text=Text.Text
			}
			).ToList();
			var TextsList = new TextsListViewModel()
			{
				TextsCount = Texts.Count,
				//TODO:添加显示方案
				PageCount = 1,
				Pages = 1,
				Texts = Texts
			};
			return View(TextsList);
		}
	}
}