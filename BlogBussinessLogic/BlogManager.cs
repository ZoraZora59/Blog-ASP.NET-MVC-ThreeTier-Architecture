using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogModel;
using BlogRepository;

namespace BlogBussinessLogic
{
    public class BlogManager
    {
		private BlogRepository.BlogRepository repository = new BlogRepository.BlogRepository();
		public List<BlogText> GetAllTexts()
		{
			return repository.GetAll();
		}
		public BlogText GetTextById(int id)
		{
			return repository.GetByID(id);
		}
    }
}
