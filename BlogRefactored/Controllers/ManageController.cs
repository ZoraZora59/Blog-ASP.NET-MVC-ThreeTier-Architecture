using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogBLL.ViewModels;

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

		#region 主界面
		[HttpGet]
		public ActionResult Index()//控制中心主界面
		{
			return View(manager.GetManageIndex());
		}
		#endregion

		#region 博客配置
		[HttpGet]
		public ActionResult Config()//读取博客配置XML文件
		{
			return View(manager.GetBlogConfig());
		}

		[HttpPost]
		public ActionResult Config(BlogConfig model)//设定博客配置文件
		{
			var  isSuccess=manager.SetBlogConfig(model);
			if (isSuccess)
				return View();//设定成功
			else
				return View();
		}
		#endregion

		#region 文章管理
		[HttpGet]
		public ActionResult TextList()//文章管理列表的显示
		{
			return View();
		}

		public JsonResult LoadTextList()//文章管理列表的JS实现
		{
			return Json(manager.GetManageTexts());
		}
		[HttpPost]
		public JsonResult DeleteText()//删除文章
		{
			try
			{
				manager.RemoveText(int.Parse(Request["TextID"].ToString()));
			}
			catch
			{
				throw;
			}
			return Json(null);
		}
		[HttpGet]
		public ActionResult Show()//文章详情
		{
			try
			{
				return View(manager.GetTextInDetail(int.Parse(Request["TextID"].ToString())));
			}
			catch
			{
				return Redirect("/Manage/TextList");
				throw;
			}
		}
		#endregion

		#region 文章编辑更新

		[HttpGet]
		public ActionResult Update()//文章更新
		{
			try
			{
				string jstID = Request["TextID"].ToString();
				if (jstID != null)
				{
					int tID = int.Parse(jstID);
					ViewBag.title = "文章更新";
					return View(manager.GetTextInUpdate(tID));
				}
			}
			catch (NullReferenceException)
			{
			}
			catch (Exception)//TODO:异常处理
			{
				throw;
			}
			ViewBag.Title = "创建文章";
			return View(new UpdateText());
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update([Bind(Include = "Id,Title,Category,Text")] UpdateText BlogText)//文章更新提交
		{
			if (ModelState.IsValid)
			{
				try
				{
					var IsUpdate = manager.UpdateText(BlogText);
					if (IsUpdate == true)//TODO:更新成功处理
					{
						return Redirect("/manage/textlist");
					}
				}
				catch
				{
					throw;
				}
			}
			return Redirect("/manage/textlist");//TODO:更新失败处理
		}
		#endregion

		#region 用户管理
		[HttpGet]
		public ActionResult ManageUser()//空的用户管理页面
		{
			return View();
		}
		
		public JsonResult LoadUsers()//加载用户列表
		{
			return Json(manager.GetManageUsers());
		}
		public JsonResult DelUsers(string Account)//删除用户
		{
			try
			{
				var IsDel = manager.RemoveUser(Account);
				if (IsDel == false)
				{
					return Json(null);//TODO:删除失败的反馈
				}
			}
			catch (Exception)
			{
				throw;
			}
			return Json(null);//页面更新
		}
		#endregion

		#region 分类管理
		[HttpGet]
		public ActionResult AddCategroy()//添加分类的页面显示
		{
			return View();
		}
		[HttpGet]
		public ActionResult CategoryDetail()//分类详情
		{
			try
			{
				return View(manager.GetCategoryDetail(Request["CategoryName"].ToString()));//TODO:修改分类详情View对应Model
			}
			catch (NullReferenceException)//路由地址异常
			{
				return Redirect("/manage/CategoryList");
			}
			catch (Exception)
			{
				return Redirect("/manage/");
			}
		}
		public JsonResult LoadCategoryList()//加载分类表
		{
			return Json(manager.GetManageCategories());
		}
		[HttpPost]
		public JsonResult DeleteCategory()//删除指定分类
		{
			try
			{
				manager.RemoveCategory(Request["CategoryName"].ToString());
			}
			catch (Exception)
			{
				throw;
			}
			return Json(null);
		}
		[HttpGet]
		public ActionResult RenameCategory()//分类更名
		{
			try
			{
				ViewBag.title = "分类重命名";
				ViewBag.Name = Request["CategoryName"].ToString();
			}
			catch (Exception)
			{
				return Redirect("/manage/CategoryList");
			}
			return View();
		}
		public JsonResult JSRenameCategory()//分类更名的JS实现
		{
			//TODO:修改返回值
			var NameString = Request["NameChanging"].ToString();
			string[] name = NameString.Split(new char[] { ',' });
			if (name[0] == "未分类")
				name[0] = null;
			manager.RenameCategory(name[0],name[1]);
			return Json(0);
		}
		[HttpGet]
		public ActionResult CategoryList()//空的分类列表
		{
			return View();
		}
		#endregion

		#region 评论管理
		[HttpGet]
		public ActionResult ManageComment()//空的评论管理页面
		{
			return View();
		}
		public JsonResult LoadComment()//加载评论管理界面的数据
		{
			return Json(manager.GetManageComments());
		}
		[HttpPost]
		public JsonResult DeleteComment()//删除评论
		{
			try
			{
				manager.RemoveComment(int.Parse(Request["CommitID"]));
			}
			catch (Exception)
			{
				throw;
			}
			return Json(null);
		}
		#endregion
	}
}