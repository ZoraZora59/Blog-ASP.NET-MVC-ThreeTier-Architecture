using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogRefactored.Controllers
{
    public class ManageController : Controller
    {
		private BlogBLL.BlogManager manager;
		public ManageController(BlogBLL.BlogManager blogManager)
		{
			this.manager=blogManager;
		}
        public ActionResult Index()
        {
            return View(manager.GetManageIndex());
        }
		public JsonResult LoadTextList()//文章管理列表的JS实现
		{
			return Json(manager.GetManageTexts());
		}
	}
}