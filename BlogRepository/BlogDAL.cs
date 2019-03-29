using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using BlogModel;

namespace BlogDAL
{
    public class BlogDAL//数据访问层
    {
		//操作模板    using (BlogContext db = new BlogContext())
		public BlogDAL(){ }
		#region 评论表相关
		public void AddCommit(BlogCommit addThis)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogCommits.Add(addThis);
				db.SaveChanges();
			}
		}
		public void DelCommitByID(int id)
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogCommits.Remove(db.BlogCommits.Find(id));
				db.SaveChanges();
			}
		}
		public void DelCommitByTextID(int tid)
		{
			using (BlogContext db = new BlogContext())
			{
				while(db.BlogCommits.FirstOrDefault(c=>c.TextID==tid)!=null)
				{
					db.BlogCommits.Remove(db.BlogCommits.First(c => c.TextID == tid));
					db.SaveChanges();
				}
			}
		}
		public BlogCommit GetCommitNew()
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogCommits.Last();
			}
		}
		public List<BlogCommit> GetCommitsByTextID(int tid)
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogCommits.Where(c => c.TextID == tid).ToList();
			}
		}
		public List<BlogCommit> GetCommitsAll()
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogCommits.ToList();
			}
		}
		#endregion
		#region 用户表相关
		public void DelUser(string account)
		{
			using (BlogContext db = new BlogContext())
			{
				List<BlogCommit> commitsList = db.BlogUsers.Find(account).Commits.ToList();
				while (commitsList != null)//删除相关评论
				{
					commitsList.ForEach(d => db.BlogCommits.Remove(d));//TODO:测试是否正常工作
					db.SaveChanges();
				}
				db.BlogUsers.Remove(db.BlogUsers.Find(account));
				db.SaveChanges();
			}
		}
		public void UpdateUser(string account,BlogUser updateThis)
		{
			using (BlogContext db = new BlogContext())
			{
				BlogUser item = db.BlogUsers.Find(account);
				item = updateThis;//安全性保证在业务逻辑层处理
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
		public BlogUser GetUserByID(int id)
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogUsers.Find(id);
			}
		}
		#endregion
		#region 文章表相关
		public void DelText(int textID)
		{
			using (BlogContext db = new BlogContext())
			{
				List<BlogCommit>commitsList = db.BlogTexts.Find(textID).Commits.ToList();
				while(commitsList!=null)//删除相关评论
				{
					commitsList.ForEach(d => db.BlogCommits.Remove(d));//TODO:测试是否正常工作
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
				item = updateThis;
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
		public DbSet<BlogCommit> BlogCommits { get; set; }
	}

	#endregion
}
