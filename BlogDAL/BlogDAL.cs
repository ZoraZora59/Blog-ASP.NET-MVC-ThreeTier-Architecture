using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using BlogModel;

namespace BlogDAL
{
    public class BlogDAL:IDAL//数据访问层
    {
		//操作模板    using (BlogContext db = new BlogContext())
		public BlogDAL(){ }
		#region 评论表相关
		public void AddComment(BlogComment addThis)//添加评论，参数为BlogComment实体
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.Add(new BlogComment { Account = addThis.Account, CommentText = addThis.CommentText, TextID = addThis.TextID });
				db.SaveChanges();
			}
		}
		
		public void DelCommentByID(int id)//删除评论，参数为评论的ID号
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.Remove(db.BlogComments.Find(id));
				db.SaveChanges();
			}
		}

		public void DelCommentByAccount(string Account)//删除某用户的所有评论，参数为用户账号
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.RemoveRange(db.BlogComments.Where(c => c.Account == Account));
				db.SaveChanges();
			}
		}

		public void DelCommentByTextID(int tid)//删除某文章下的所有评论，参数为文章ID号
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.RemoveRange(db.BlogComments.Where(c => c.TextID == tid));
				db.SaveChanges();
			}
		}

        public List<BlogComment> GetCommentByAccount(string userAccount)
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogComments.Where(c => c.Account.Contains(userAccount)).ToList();
               
            }
        }

        public List<BlogComment> GetCommentByAccountAndName(string userAccount, string userName)
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogComments.Where(c => c.Account.Contains(userAccount)).Where(c=>c.User.Name.Contains(userName)).ToList();

            }
        }

        public List<BlogComment> GetCommentByName(string userName)
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogComments.Where(c => c.User.Name.Contains(userName)).ToList();
            }
        }


        //public BlogComment GetCommitNew()//获取最新评论
        //{
        //	using (BlogContext db = new BlogContext())
        //	{

        //              return db.BlogCommits.Last();

        //	}
        //}

        public List<BlogComment> GetCommitsByTextID(int tid)//获取某文章下的所有评论，参数为文章ID号，返回一个BlogComment格式的列表
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogComments.Where(c => c.TextID == tid).ToList();
			}
		}

		public List<BlogComment> GetCommentsAll()//获取全站所有评论，返回一个BlogComment格式的列表
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogComments.ToList();
			}
		}
		#endregion
		#region 用户表相关
		public void DelUser(string account)//删除某用户，参数为用户账号
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogComments.RemoveRange(db.BlogComments.Where(c => c.Account == account));
				db.BlogUsers.Remove(db.BlogUsers.Find(account));
				db.SaveChanges();
			}
		}

		public void UpdateUser(string account,BlogUser updateThis)//修改用户信息，参数为（用户账号，用于更新的BlogUser实体）
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
        
        public IQueryable<BlogText> GetTexts()
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogTexts.AsQueryable();
            };
        }
        
        public void AddUser(BlogUser addThis)//添加用户（用户注册），参数为新用户的BlogUser实体
		{
			using (BlogContext db = new BlogContext())
			{
				db.BlogUsers.Add(addThis);
				db.SaveChanges();
			}
		}

		public List<BlogUser> GetUsersAll()//获取全站所有用户信息，返回一个BlogUser类型的列表
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogUsers.ToList();
			}
		}

		public BlogUser GetUserByAccount(string Account)//获取某用户信息，参数为用户账号，返回一个带有数据的BlogUser实体，如有异常应在上层处理
		{
			using (BlogContext db = new BlogContext())
			{
                return db.BlogUsers.FirstOrDefault(c => c.Account==Account);
			}
		}

        public List<BlogUser> GetUserByName(string userName)//根据名字获取用户（模糊），返回BlogUser列表
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogUsers.Where(m => m.Name.Contains(userName)).ToList();
            }
        }

        public List<BlogUser> GetUserByAccountBlur(string userAccount)//根据用户名获取用户（模糊），返回BlogUser列表
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogUsers.Where(m => m.Account.Contains(userAccount)).ToList();
            }
        }

        public List<BlogUser> GetUserByAccountAndName(string userAccount,string userName)//根据用户名和名字获取用户（模糊），返回Bloguser列表
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogUsers.Where(m => m.Name.Contains(userName)).Where(m => m.Account.Contains(userAccount)).ToList();
            }
        }
        #endregion
        #region 文章表相关

        public void AddTextHot(int id)//增加文章点击量，参数为文章ID号
        {
            using (BlogContext db = new BlogContext())
            {
				BlogText item = db.BlogTexts.Find(id);
				item.Hot++;
				db.SaveChanges();
            }
        }

        public void DelText(int textID)//删除某文章，参数为文章ID号
		{
			using (BlogContext db = new BlogContext())
			{
				//先修改链表
				var Removing=db.BlogTexts.First(c => c.TextID == textID);
				if(Removing.NexID==0)
				{
					if(Removing.PreID==0)//独苗
					{
					}
					else//最后一个
					{
						db.BlogTexts.Find(Removing.PreID).NexID = 0;
					}
				}
				else
				{
					if(Removing.PreID==0)//第一个
					{
						db.BlogTexts.Find(Removing.NexID).PreID = 0;
					}
					else//中间的
					{
						db.BlogTexts.Find(Removing.NexID).PreID = Removing.PreID;
						db.BlogTexts.Find(Removing.PreID).NexID = Removing.NexID;
					}
				}
				db.BlogComments.RemoveRange(db.BlogComments.Where(c => c.TextID == textID));
				db.BlogTexts.Remove(db.BlogTexts.Find(textID));
				db.SaveChanges();
			}
		}

		public void AddText(BlogText addThis)//新增文章，参数为BlogText的实体
		{
			using (BlogContext db = new BlogContext())
			{
				BlogText before=new BlogText();//连接前后文
				before = db.BlogTexts.ToList().LastOrDefault();
				var newBlog=db.BlogTexts.Add(new BlogText { TextTitle = addThis.TextTitle, CategoryName = addThis.CategoryName, Text = addThis.Text, FirstView = addThis.FirstView ,PreID=before.TextID});
				db.SaveChanges();
				if (before != null)
					db.BlogTexts.Find(before.TextID).NexID = newBlog.TextID;
				db.SaveChanges();
			}
		}

		public void UpdateText(int textID,BlogText updateThis)//更新文章，参数为（文章ID号，更新后的文章BlogText）
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

		public BlogText GetTextByID(int id)//获取某文章信息，参数为文章的ID号，返回一个BlogText实体
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogTexts.Find(id);
			}
		}

		public List<BlogText> GetTextsAll()//获取全站所有文章信息，返回一个BlogText类型的列表
		{
			using (BlogContext db = new BlogContext())
			{
				return db.BlogTexts.ToList();
			}
		}

        public List<BlogText> searchblogByTitle(string textTitle)//搜索文章，通过题目搜索
        {
            using (BlogContext db = new BlogContext())
            {
                return db.BlogTexts.Where(m => m.TextTitle.Contains(textTitle)).ToList();
            }
            
        }

        
        #endregion
        #region 服务器文件处理
        public void DelFile(string url)
		{
			File.Delete(url);
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
