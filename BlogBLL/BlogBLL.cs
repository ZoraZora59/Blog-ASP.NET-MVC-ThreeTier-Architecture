using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogModel;
using BlogDAL;
#region 必读内容

/// <summary>
///		这里仅作为测试DEMO
///
///		实际项目并不使用这部分内容
///
///		TODO:测试完了删掉
/// </summary>

namespace BlogBLL
{
	public class BlogBLL : IBLL
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

#endregion

