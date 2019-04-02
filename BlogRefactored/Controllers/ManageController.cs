using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogRefactored.Controllers
{
    public class ManageController : Controller
    {
		#region AutoFac依赖注入
		private BlogBLL.BlogManager manager;
		public ManageController(BlogBLL.BlogManager blogManager)
		{
			this.manager = blogManager;
		}
		#endregion

		public ActionResult Index()//控制中心主界面
        {
            return View(manager.GetManageIndex());
        }
		public JsonResult LoadTextList()//文章管理列表的JS实现
		{
			return Json(manager.GetManageTexts());
		}
	}
}