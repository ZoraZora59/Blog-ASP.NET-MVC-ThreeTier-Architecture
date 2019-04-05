using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogBLL.ViewModels;
using BlogModel;
using System.Web.Helpers;

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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //防止为登录进入后台界面
            base.OnActionExecuting(filterContext);
            var currentLoginUser = Session["loginuser"] == null ? null : (BlogUser)Session["loginuser"];
            if (currentLoginUser == null)//如果没有登录进后台跳转到登录界面
            {
                filterContext.Result = Redirect("/home/Login");
            }
            else//如果是普通用户，跳转到主界面
            {
                if (currentLoginUser.Account != "admin123")
                {
                    filterContext.Result = Redirect("/");
                }
            }
            
        }
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
		public ActionResult SetConfig()//设定博客配置文件
		{
			string configTitle = Request["Title"].ToString();
			string configSign = Request["Sign"].ToString();
			string configNote = Request["Note"].ToString();
			var model = new BlogConfig { Name = configTitle, Note = configNote, Sign = configSign };
			bool isSuccess = manager.SetBlogConfig(model);
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

		public JsonResult LoadTextList(int page,int rows,string sort, string order,string TextTitle)//文章管理列表的JS实现
		{
            var result = new { total = manager.GetTextNum(), rows = manager.GetManageTexts(page, rows, sort,order, TextTitle) };
			return Json(result);
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
				return Json(null);
			}
			return Json(0);
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
			ViewBag.Title = "创建文章";
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
			catch (Exception)
			{
			}
			return View(new UpdateText());
		}

		//[HttpPost]
		//[ValidateInput(false)]
		//public ActionResult Update([Bind(Include = "Id,Title,Category,Text")] UpdateText BlogText)//文章更新提交
		//{
		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			var IsUpdate = manager.UpdateText(BlogText);
		//			if (IsUpdate == true)//TODO:更新成功处理
		//			{
		//				return Redirect("/manage/textlist");
		//			}
		//		}
		//		catch
		//		{
		//			throw;
		//		}
		//	}
		//	return Redirect("/manage/textlist");//TODO:更新失败处理
		//}
		[HttpPost]
		[ValidateInput(false)]
		public JsonResult UpdateText()
		{
			if (ModelState.IsValid)
			{
				try
				{
					int BlogID=int.Parse(Request["ID"].ToString());
                    string BlogTitle = Request["Title"].ToString();
                    string BlogCategory = Request["Categroy"].ToString();
                    string BlogContent = Request.Unvalidated["Content"].ToString();
                    var model = new UpdateText { Id=BlogID, Category = BlogCategory, Title = BlogTitle, Text = BlogContent };
					var IsUpdate = manager.UpdateText(model);
					if (IsUpdate == true)
					{
						return Json(0);
					}
					else
					{
						return Json(null);
					}
				}
				catch
				{
					return Json(null);
				}
			}
			return Json(null);
		}
		#endregion

		#region 用户管理
		[HttpGet]
		public ActionResult ManageUser()//空的用户管理页面
		{
			return View();
		}
		
		public JsonResult LoadUsers(int page, int rows)//加载用户列表
		{
            var result = new { total = manager.GetUserNum(), rows = manager.GetManageUsers(page, rows) };
            return Json(result);
		}
		public JsonResult DelUsers(string Account)//删除用户
		{
			try
			{
				var IsDel = manager.RemoveUser(Account);
				if (IsDel == false)
				{
					return Json(null);
				}
				else return Json(0);
			}
			catch (Exception)
			{
				return Json(null);
			}
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
				return View(manager.GetCategoryDetail(Request["CategoryName"].ToString()));
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
		public JsonResult LoadCategoryList(int page,int rows)//加载分类表
		{
            var result = new { total = manager.GetCateNum(), rows = manager.GetManageCategoriesInPage(page, rows) };
            return Json(result);
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
				return Json(null);
			}
			return Json(0);
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
			var NameString = Request["NameChanging"].ToString();
			string[] name = NameString.Split(new char[] { ',' });
			if (name[0] == "未分类")
				name[0] = string.Empty;
			if (manager.RenameCategory(name[0], name[1]))
				return Json(0);
			return Json(null);
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
		public JsonResult LoadComment(int page,int rows)//加载评论管理界面的数据
		{
            var result = new { total = manager.GetCommentNum(), rows = manager.GetManageComments(page, rows) };
            return Json(result);
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
				return Json(null);
			}
			return Json(0);
		}
		#endregion

		#region Kindeditor上传
		public ActionResult Upload()
		{
			//文件保存目录路径
			String savePath = "/attached/";
			//文件保存目录URL
			String saveUrl = "/attached/";
			//定义允许上传的文件扩展名
			Hashtable extTable = new Hashtable
			{
				{ "image", "gif,jpg,jpeg,png,bmp" },
				{ "flash", "swf,flv" },
				{ "media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb" },
				{ "file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2" }
			};
			//最大文件大小
			int maxSize = 1000000;
			HttpPostedFileBase imgFile = Request.Files["imgFile"];
			if (imgFile == null)
			{
				return Content("请选择文件。");
			}
			String dirPath = Server.MapPath(savePath);
			if (!Directory.Exists(dirPath))
			{
				return Content("上传目录不存在。");
			}
			String dirName = Request.QueryString["dir"];
			if (String.IsNullOrEmpty(dirName))
			{
				dirName = "image";
			}
			if (!extTable.ContainsKey(dirName))
			{
				return Content("目录名不正确。");
			}
			String fileName = imgFile.FileName;
			String fileExt = Path.GetExtension(fileName).ToLower();
			if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
			{
				return Content("上传文件大小超过限制。");
			}
			if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
			{
				return Content("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
			}
			//创建文件夹
			dirPath += dirName + "/";
			saveUrl += dirName + "/";
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
			dirPath += ymd + "/";
			saveUrl += ymd + "/";
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
			String filePath = dirPath + newFileName;
			imgFile.SaveAs(filePath);
			String fileUrl = saveUrl + newFileName;
			Hashtable hash = new Hashtable
			{
				["error"] = 0,
				["url"] = fileUrl
			};
			return Json(hash);
		}
		#endregion
	}
}