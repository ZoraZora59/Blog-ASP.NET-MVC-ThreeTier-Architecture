using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogModel;

namespace BlogRepository
{
    public class BlogRepository
    {
		private List<BlogText> textsLists;
		public BlogText GetByID(int id)
		{
			return textsLists.Where(c => c.TextID == id).FirstOrDefault();
		}
		public List<BlogText> GetAll()
		{
			return textsLists;
		}
    }
}
