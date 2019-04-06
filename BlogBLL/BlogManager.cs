using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using BlogBLL.ViewModels;
using BlogBLL.App_Code;
using BlogModel;

namespace BlogBLL
{
	public class BlogManager : IBLL
	{
		private BlogDAL.BlogDAL repository = new BlogDAL.BlogDAL();

		public ManageMain GetManageIndex()//获取管理主页数据
		{
			int hot = 0;
			foreach (var item in repository.GetTextsAll())//统计点击量
			{
				hot += item.Hot;
			}
			var Mmain = new ManageMain//汇总其他数据
			{
				UserCount = repository.GetUsersAll().Count(),
				TextCount = repository.GetTextsAll().Count(),
				CommentCount = repository.GetCommentsAll().Count(),
				HotCount = hot
			};
			return Mmain;
		}
		public List<ManageComment> GetManageComments(int page,int rows)//评论列表数据
		{
			List<ManageComment> CommentsList = new List<ManageComment>();
			List<BlogModel.BlogComment> trans = repository.GetCommentsAll();
			List<BlogModel.BlogUser> tempUsersList = repository.GetUsersAll();
            var totalComment = repository.GetCommentsAll().Count();
            for (int i = (page - 1) * rows; i < page * rows; i++)
            {
                if (i >= totalComment)
                {
                    break;
                }
				ManageComment temp = new ManageComment
				{
					Account = trans[i].Account,
					Id = trans[i].CommmentID,
					Name = tempUsersList.Find(c => c.Account == trans[i].Account).Name,
					TextId = trans[i].TextID,
					Content = trans[i].CommentText,
					Date = trans[i].CommentChangeDate.ToString()
				};
				CommentsList.Add(temp);
			}
			return CommentsList;
		}
        public List<ManageCategory> GetManageCategories()//获取分类列表数据,用于主页显示分类
        {
            List<ManageCategory> Categories = new List<ManageCategory>();
            List<BlogModel.BlogText> blogTexts = repository.GetTextsAll();
            foreach (var item in blogTexts)
            {
                ManageCategory addOne = new ManageCategory
                {
                    CategoryName = item.CategoryName,
                    CategoryHot = item.Hot,
                    TextCount = 1
                };
                if (!Categories.Exists(c => c.CategoryName == addOne.CategoryName))
                {
                    Categories.Add(addOne);
                }
                else
                {
                    var temp = Categories.Find(c => c.CategoryName == addOne.CategoryName);
                    temp.CategoryHot += addOne.CategoryHot;
                    temp.TextCount += addOne.TextCount;
                }
            }
            Categories = Categories.OrderByDescending(c => c.CategoryHot).ToList();
            return Categories;
        }
        public List<ManageCategory> GetManageCategoriesInPage(GridPager gp)//分页获取分类列表数据
		{
            Console.WriteLine(gp.page);
			List<ManageCategory> Categories = new List<ManageCategory>();//用于保存所有的分类
            List<ManageCategory> TempCategories = new List<ManageCategory>();//用于保存显示出分页的分类内容

            List<BlogModel.BlogText> blogTexts = repository.GetTextsAll();
            var totalCatenum = new BlogSide().GetCateString().Count();
            foreach(var item in blogTexts)//1.首先获取所有不重复的分类列表
            {
                ManageCategory addOne = new ManageCategory
                {
                    CategoryName = item.CategoryName,
                    CategoryHot = item.Hot,
                    TextCount = 1
                };
                if (!Categories.Exists(c => c.CategoryName == addOne.CategoryName))
                {
                    Categories.Add(addOne);
                }
                else
                {
                    var temp = Categories.Find(c => c.CategoryName == addOne.CategoryName);
                    temp.CategoryHot += addOne.CategoryHot;
                    temp.TextCount += addOne.TextCount;
                }
            }
            switch (gp.sort)//2.根据分类列表，进行排序
            {
                case "TextCount":
                    if (gp.order == "desc")
                    {
      
                        Categories = Categories.OrderByDescending(c => c.TextCount).ToList();
                    }
                    else
                    {
                        Categories = Categories.OrderBy(c => c.TextCount).ToList();
                    }
                    break;
                case "CategoryHot":
                    if (gp.order == "desc")
                    {
                        Categories = Categories.OrderByDescending(c => c.CategoryHot).ToList();
                    }
                    else
                    {
                        Categories = Categories.OrderBy(c => c.CategoryHot).ToList();
                    }
                    break;
                default:
                    Categories = Categories.OrderByDescending(c => c.CategoryHot).ToList();
                    break;
            }
            for (int i = (gp.page - 1) * gp.rows; i < gp.page * gp.rows; i++)//3.通过分页显示内容
            {
                if (i >= totalCatenum)
                {
                    break;

                }
				ManageCategory addOne = new ManageCategory
				{
					CategoryName = Categories[i].CategoryName,
					CategoryHot = Categories[i].CategoryHot,
					TextCount = Categories[i].TextCount
                };
                TempCategories.Add(addOne);
			}
            
			return TempCategories;
		}

        public int GetTextNum()
        {
            return repository.GetTextsAll().Count();
        }

        public List<ManageText> GetManageTexts(GridPager gp, string TextTitle)//获取文章列表数据
		{
			List<ManageText> manageTexts = new List<ManageText>();
            //var totalpage = repository.GetTextsAll().Count();
            var trans = new List<BlogText>();
            if (!string.IsNullOrEmpty(TextTitle))
            {
                trans = repository.searchblogByTitle(TextTitle);
                switch (gp.sort)//使用switch,考虑到可扩展成其他类型的排序
                {
                    case "Hot":
                        if (gp.order == "desc")//在一个case中判断是正序还是倒序
                        {
                            trans = repository.searchblogByTitle(TextTitle).OrderByDescending(m => m.Hot).ToList();
                        }
                        else
                        {
                            trans = repository.searchblogByTitle(TextTitle).OrderBy(m => m.Hot).ToList();
                        }
                        break;
                    default://默认情况，当sort为空
                        trans = repository.searchblogByTitle(TextTitle);
                        break;
                }
            }
            else
            {
                switch (gp.sort)//使用switch,考虑到可扩展成其他类型的排序
                {
                    case "Hot":
                        if (gp.order == "desc")//在一个case中判断是正序还是倒序
                        {
                            trans = repository.GetTextsAll().OrderByDescending(m => m.Hot).ToList();
                        }
                        else
                        {
                            trans = repository.GetTextsAll().OrderBy(m => m.Hot).ToList();
                        }
                        break;
                    default://默认情况，当sort为空
                        trans = repository.GetTextsAll();
                        break;
                }
            }
           


            //var trans = repository.GetTextsAll();
            for (int i = (gp.page -1)* gp.rows;i< gp.page * gp.rows;i++)
			//foreach (var item in trans)
			{
                if (i >= trans.Count())
                {
                    break;
                }
				if (trans[i].CategoryName == string.Empty)
                    trans[i].CategoryName = "未分类";
				ManageText temp = new ManageText
				{
					TextID = trans[i].TextID,
					TextTitle = trans[i].TextTitle,
					CategoryName = trans[i].CategoryName,
					TextChangeDate = trans[i].TextChangeDate.ToString(),
					Hot = trans[i].Hot
				};
				manageTexts.Add(temp);
			}
			return manageTexts;
		}
		public List<ManageUser> GetManageUsers(GridPager gp,string UserAccount,string UserName)//获取用户列表数据
		{
			List<ManageUser> manageUsers = new List<ManageUser>();//用于存放所有用户
            List<ManageUser> TempmanageUsers = new List<ManageUser>();//用于显示用户
            List<BlogUser> trans = new List<BlogUser>();
            var totaluser = repository.GetUsersAll().Count();
            if (!string.IsNullOrEmpty(UserAccount))//如果Account存在，就直接查询。注意：Account不是模糊搜索，必须输入所有的字符
            {
                trans.Add(repository.GetUserByAccount(UserAccount));
                if (trans[0] == null||UserAccount=="admin123")//如果没有此用户，直接返回NULL，不能查找管理员用户
                {
                    return manageUsers;
                }
            }
            if (!string.IsNullOrEmpty(UserName))//如果Name存在就搜索Name,昵称Name是模糊搜索
            {
                trans = repository.GetUserByName(UserName);
                trans.Remove(trans.Find(c => c.Account == "admin123"));//不能查找管理员用户
            }
            if (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(UserAccount))//如果都为空，则默认
            {
                trans = repository.GetUsersAll();
                trans.Remove(trans.Find(c => c.Account == "admin123"));//不能查找管理员用户
            }
            foreach (var item in trans)
            {
                ManageUser temp = new ManageUser
                {
                    Account = item.Account,
                    Name = item.Name,
                    CommmentCount = 0
                };
                var cmtlist = repository.GetCommentsAll().Where(c => c.Account == temp.Account).ToList();
                foreach (var cmt in cmtlist)
                {
                    temp.CommmentCount++;
                }
                manageUsers.Add(temp);
            }

            switch (gp.sort)//使用switch,考虑到可扩展成其他类型的排序
            {
                case "CommmentCount":
                    if (gp.order == "desc")//在一个case中判断是正序还是倒序
                    {
                        manageUsers = manageUsers.OrderByDescending(m => m.CommmentCount).ToList();
                    }
                    else
                    {
                        manageUsers = manageUsers.OrderBy(m => m.CommmentCount).ToList();
                    }
                    break;
                default://默认情况，当sort为空
                    break;
            }

            for (int i = (gp.page - 1) * gp.rows; i < gp.page * gp.rows; i++)//根据分页显示用户
            {
                if (manageUsers.Count == 0)
                {
                    break;
                }
                if (i >= trans.Count())
                {
                    break;
                }
                
                ManageUser temp = new ManageUser
                {
                    Account = manageUsers[i].Account,
                    Name = manageUsers[i].Name,
                    CommmentCount = manageUsers[i].CommmentCount
                };
                TempmanageUsers.Add(temp);
            }
            return TempmanageUsers;

        }
		public UpdateText GetTextInUpdate(int tid)//获取待编辑博文的内容
		{
			try
			{
				var uTxt = repository.GetTextsAll().Find(c => c.TextID == tid);
				UpdateText updateText = new UpdateText
				{
					Id = uTxt.TextID,
					Category = uTxt.CategoryName,
					Text = uTxt.Text,
					Title = uTxt.TextTitle
				};
				return updateText;
			}
			catch (System.ArgumentNullException)//前后端不同步时返回空
			{
				return null;
			}
		}
		public DetailCategory GetCategoryDetail(string categoryName)//获取分类详情
		{
			DetailCategory category = new DetailCategory();
			category.Texts = new List<TextsBelone>();
			category.Category = GetManageCategories().Find(c => c.CategoryName == categoryName);
			var texts = repository.GetTextsAll().Where(c=>c.CategoryName==categoryName);
			foreach(var item in texts)
			{
				var textsBelong = new TextsBelone
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					Hot = item.Hot,
					ChangeTime = item.TextChangeDate.ToString()
				};
				category.Texts.Add(textsBelong);
			}
			return category;
		}

        public object GetCateNum()
        {
            return new BlogSide().GetCateString().Count();
        }

        public int GetUserNum()
        {
            return repository.GetUsersAll().Count();
        }

        public BlogModel.BlogText GetTextInDetail(int tid)//获取文章详情
		{
			try
			{
				return repository.GetTextByID(tid);
			}
			catch
			{
				return null;
			}
		}
		public BlogConfig GetBlogConfig()//获取博客配置信息
		{
			return new SerializeTool().DeSerialize<BlogConfig>();
		}

		public bool SetBlogConfig(BlogConfig cfg)//设定博客配置文件
		{
			bool isSuccess = false;
			try
			{
				new SerializeTool().Serialize<BlogConfig>(cfg);
				isSuccess = true;
			}
			catch
			{
			}
			return isSuccess;
		}
		public bool RenameCategory(string oldName,string  newName)//分类重命名
		{
			bool isSuccess = false;
			try
			{
				var txtList = repository.GetTextsAll().Where(c => c.CategoryName == oldName).ToList();
				foreach (var item in txtList)
				{
					item.CategoryName = newName;
					repository.UpdateText(item.TextID, item);
				}
				isSuccess = true;
			}
			catch
			{
			}
			return isSuccess;
		}
		public bool RemoveCategory(string name)//删除分类
		{
			return RenameCategory(name, "未分类");
		}
		public bool RemoveText(int tid)//删除博文
		{
			bool isSuccess = false;
			try
			{
				repository.DelText(tid);
				repository.DelCommentByTextID(tid);
				isSuccess = true;
			}
			catch
			{
			}
			return isSuccess;
		}

        public int GetCommentNum()
        {
            return repository.GetCommentsAll().Count();
        }

        public bool RemoveUser(string account)//删除用户
		{
			bool isSuccess = false;
			try
			{
				repository.DelCommentByAccount(account);
				repository.DelUser(account);
				isSuccess = true;
			}
			catch
			{
			}
			return isSuccess;
		}
		public bool UpdateText(UpdateText blogText)//更新文章
		{
			bool isSuccess = false;
			try
			{
				BlogModel.BlogText blog = new BlogModel.BlogText
				{
					Text = blogText.Text,
					FirstView = getFirstView(blogText.Text),
					TextTitle = blogText.Title,
					CategoryName = blogText.Category
				};
				if(string.IsNullOrEmpty(blog.CategoryName))
				{
					blog.CategoryName = "未分类";
				}
				if (blogText.Id==0)//新增文章
				{
					repository.AddText(blog);
				}
				else//修改文章
				{
					blog.TextID = blogText.Id;
					repository.UpdateText(blog.TextID, blog);
				}
				isSuccess = true;
			}
			catch
			{
			}
			return isSuccess;
		}
		public bool RemoveComment(int cid)//删除评论
		{
			bool isSuccess = false;
			try
			{
				repository.DelCommentByID(cid);
				isSuccess = true;
			}
			catch
			{
			}
			return isSuccess;
		}

		private string getFirstView(string content)//获取摘要
		{
			content = Regex.Replace(content, "<[^>]+>", "");
			content = Regex.Replace(content, "&[^;]+;", "");
			if (content.Length < 201)
			{
				return content;
			}
			else
			{
				content = content.Substring(0, 200);
			}

			return content;
		}

		#region KindEditor文件上传
		public bool UploadFiles(HttpPostedFileBase imgFile, string dirPath , string dirName)//文件上传
		{
			bool isSuccess = false;
			//文件保存目录路径
			//通过Controller传入		String savePath = "/attached/";  TODO:在Controller中附加到dirPath字符串里
			//文件保存目录URL
			string saveUrl = "/attached/";
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
			//通过Controller传入		HttpPostedFileBase imgFile = Request.Files["imgFile"];
			if (imgFile == null)
			{
				return isSuccess;
			}
			//通过Controller传入		String dirPath = Server.MapPath(savePath);
			if (!Directory.Exists(dirPath))
			{
				return isSuccess;
			}
			//通过Controller传入		String dirName = Request.QueryString["dir"];
			if (string.IsNullOrEmpty(dirName))
			{
				dirName = "image";
			}
			if (!extTable.ContainsKey(dirName))
			{
				return isSuccess;
			}
			string fileName = imgFile.FileName;
			string fileExt = Path.GetExtension(fileName).ToLower();
			if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
			{
				return isSuccess;
			}
			if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(((string)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
			{
				return isSuccess;
			}
			//创建文件夹
			dirPath += dirName + "/";
			saveUrl += dirName + "/";
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
			dirPath += ymd + "/";
			saveUrl += ymd + "/";
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
			string filePath = dirPath + newFileName;
			imgFile.SaveAs(filePath);
			string fileUrl = saveUrl + newFileName;
			Hashtable hash = new Hashtable
			{
				["error"] = 0,
				["url"] = fileUrl
			};
			return isSuccess;
		}
		#endregion
	}
}