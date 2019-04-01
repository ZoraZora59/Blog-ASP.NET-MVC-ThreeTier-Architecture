using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogModel;
using BlogDAL;

namespace BlogBLL
{
    public class BlogBLL:IBLL
    {
		private BlogDAL.BlogDAL repository = new BlogDAL.BlogDAL();
		public List<BlogText> GetAllTexts()
		{
			return repository.GetTextsAll();
		}
		public BlogText GetTextById(int id)
		{
			return repository.GetTextByID(id);
		}
    }
}
