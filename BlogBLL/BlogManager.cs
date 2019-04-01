using System.Collections.Generic;
using System.Linq;
using BlogBLL.ViewModels;
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
		public List<ManageComment> GetManageComments()//获取评论列表数据
		{
			List<ManageComment> CommentsList = new List<ManageComment>();
			List<BlogModel.BlogComment> trans = repository.GetCommentsAll();
			List<BlogModel.BlogUser> tempUsersList = repository.GetUsersAll();
			foreach (var item in trans)
			{
				ManageComment temp = new ManageComment
				{
					Account = item.Account,
					Id = item.CommmentID,
					Name = tempUsersList.Find(c => c.Account == item.Account).Name,
					TextId = item.TextID,
					Content = item.CommentText,
					Date = item.CommentChangeDate.ToString()
				};
				CommentsList.Add(temp);
			}
			return CommentsList;
		}
		public List<ManageCategory> GetManageCategories()//获取分类列表数据
		{
			List<ManageCategory> Categories = new List<ManageCategory>();
			List<BlogModel.BlogText> blogTexts = repository.GetTextsAll();
			foreach(var item in blogTexts)
			{
				if(item.CategoryName==null)
				{
					item.CategoryName = "未分类";
				}
				ManageCategory addOne = new ManageCategory
				{
					CategoryName = item.CategoryName,
					CategoryHot = item.Hot,
					TextCount = 1
				};
				if(!Categories.Exists(c=>c.CategoryName==addOne.CategoryName))
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
		public List<ManageText> GetManageTexts()//获取文章列表数据
		{
			List<ManageText> manageTexts = new List<ManageText>();
			var trans = repository.GetTextsAll();
			foreach (var item in trans)
			{
				ManageText temp = new ManageText
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					CategoryName = item.CategoryName,
					TextChangeDate = item.TextChangeDate.ToString(),
					Hot = item.Hot
				};
				manageTexts.Add(temp);
			}
			return manageTexts;
		}
		public List<ManageUser> GetManageUsers()//获取用户列表数据
		{
			List<ManageUser> manageUsers = new List<ManageUser>();
			var trans = repository.GetUsersAll();
			trans.Remove(trans.Find(c => c.Account == "admin123"));
			foreach(var item in trans)
			{
				ManageUser temp = new ManageUser
				{
					Account = item.Account,
					Name = item.Name,
					CommmentCount = 0
				};
				var cmtlist = repository.GetCommentsAll().Where(c=>c.Account==temp.Account).ToList();
				foreach(var cmt in cmtlist)
				{
					temp.CommmentCount++;
				}
				manageUsers.Add(temp);
			}
			return manageUsers;
		}
	}
}