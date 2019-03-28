using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BlogModel;

namespace BlogRepository
{
    public class BlogRepository
    {
		public BlogRepository()
		{
			this.textsLists = new BlogContext().BlogTexts.ToList();
		}
		private List<BlogText> textsLists;
		public BlogText GetByID(int id)
		{
			return textsLists.Find(c => c.TextID == id);
		}
		public List<BlogText> GetAll()
		{
				return this.textsLists;
		}
    }
	//[DbConfigurationType(typeof(System.Data.Entity.SqlServer.SqlProviderServices))]//添加与MSSQL类型相关的组件(默认)
	public class BlogContext : DbContext
	{
		public BlogContext() : base("name=BlogContext")
		{ }
		public DbSet<BlogText> BlogTexts { get; set; }
	}
}
