using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using BlogModel;

namespace BlogDAL
{
    public class BlogDAL:IDAL//数据访问层
    {
		//操作模板    using (BlogContext db = new BlogContext())
		public BlogDAL(){ }
		#region 评论表相关
		public void AddComment(BlogComment addThis)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.Add(addThis);
				db.SaveChanges();
			}
		}
		public void DelCommentByID(int id)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.Remove(db.BlogComments.Find(id));
				db.SaveChanges();
			}
		}
		public void DelCommentByAccount(string Account)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.RemoveRange(db.BlogComments.Where(c => c.Account == Account));
				db.SaveChanges();
			}
		}
		public void DelCommentByTextID(int tid)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.RemoveRange(db.BlogComments.Where(c => c.TextID == tid));
				db.SaveChanges();
			}
		}
		//public BlogComment GetCommitNew()
		//{
		//	using (BlogContext db = new BlogContext())
		//	{
                
  //              return db.BlogCommits.Last();
                
		//	}
		//}
		public List<BlogComment> GetCommitsByTextID(int tid)
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogComments.Where(c => c.TextID == tid).ToList();
			}
		}
		public List<BlogComment> GetCommentsAll()
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogComments.ToList();
			}
		}
		#endregion
		#region 用户表相关
		public void DelUser(string account)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.RemoveRange(db.BlogComments.Where(c => c.Account == account));
				db.BlogUsers.Remove(db.BlogUsers.Find(account));
				db.SaveChanges();
			}
		}
		public void UpdateUser(string account,BlogUser updateThis)
		{
			using (BlogContext db = new BlogContext())
			{
				BlogUser item = db.BlogUsers.Find(account);
				//Warning:安全性保证在业务逻辑层处理
				item.Name = updateThis.Name;
				item.Password = updateThis.Password;
				db.SaveChanges();
			}
		}
		public void AddUser(BlogUser addThis)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogUsers.Add(addThis);
				db.SaveChanges();
			}
		}
		public List<BlogUser> GetUsersAll()
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogUsers.ToList();
			}
		}
		public BlogUser GetUserByAccount(string Account)
		{
			using (BlogContext db = new BlogContext())
			{
                return db.BlogUsers.FirstOrDefault(c => c.Account==Account);
			}
		}
		#endregion
		#region 文章表相关
		public void DelText(int textID)
		{
			using (BlogContext db = new BlogContext())
			{
				List<BlogComment>commitsList = db.BlogTexts.Find(textID).Commits.ToList();
				while(commitsList!=null)//删除相关评论
				{
					commitsList.ForEach(d => db.BlogComments.Remove(d));//TODO:测试是否正常工作
					db.SaveChanges();
				}
				db.BlogTexts.Remove(db.BlogTexts.Find(textID));
				db.SaveChanges();
			}
		}
		public void AddText(BlogText addThis)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogTexts.Add(addThis);
				db.SaveChanges();
			}
		}
		public void UpdateText(int textID,BlogText updateThis)
		{
			using (BlogContext db = new BlogContext())
			{
				BlogText item = db.BlogTexts.Find(textID);
				item.CategoryName = updateThis.CategoryName;
				item.Text = updateThis.Text;
				item.TextTitle = updateThis.TextTitle;
				item.FirstView = updateThis.FirstView;
				db.SaveChanges();
			}
		}
		public BlogText GetTextByID(int id)
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogTexts.Find(id);
			}
		}
		public List<BlogText> GetTextsAll()
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogTexts.ToList();
			}
		}
		#endregion
	}
	#region 数据库上下文组件
	//[DbConfigurationType(typeof(System.Data.Entity.SqlServer.SqlProviderServices))]//添加与MSSQL类型相关的组件(默认)
	public class BlogContext : DbContext
	{
		public BlogContext() : base("name=BlogContext")
		{ }
		public DbSet<BlogText> BlogTexts { get; set; }
		public DbSet<BlogUser> BlogUsers { get; set; }
		public DbSet<BlogComment> BlogComments { get; set; }
	}

	#endregion
}
