using BlogBLL.ViewModels;
using BlogBLL.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;


namespace BlogBLL
{
	public class BlogGuests:IBLL
	{
		private BlogDAL.BlogDAL repository = new BlogDAL.BlogDAL();
        public bool Regist (ViewModels.RegisterUser addOne)
        {
            var isRegist = false;
            var res = repository.GetUserByAccount(addOne.Account);
            if (res == null)
            {
                BlogModel.BlogUser NewBlog = new BlogModel.BlogUser();
                NewBlog.Account = addOne.Account;
                NewBlog.Password = md5tool.GetMD5(addOne.Password);
                NewBlog.Name = addOne.Name;
                repository.AddUser(NewBlog);
                isRegist = true;
                return isRegist;
            }
            return isRegist;
            
        }
        public BlogModel.BlogUser Login (ViewModels.LoginUser LogOne)
        {

            BlogModel.BlogUser LoginData = new BlogModel.BlogUser();
            LogOne.Password = md5tool.GetMD5(LogOne.Password);
            BlogModel.BlogUser temp = repository.GetUserByAccount(LogOne.Account);
            if (temp.Password == LogOne.Password)
            {
                LoginData= temp;
            }
            return LoginData;
        }

        public object GetIndex(int? page)
        {
            var models = new List<TextIndex>();
            List<BlogModel.BlogText> TextList = repository.GetTextsAll();
            foreach (var item in TextList)
            {
                var temp = new TextIndex
                {
                    TextID = item.TextID,
                    CommitCount = TextList.Where(c => c.TextID == item.TextID).Count(),
                    Text = item.Text,
                    FirstView = item.FirstView
                };
                if (item.CategoryName == null)
                    item.CategoryName = "未分类";
                temp.CategoryName = item.CategoryName;
                temp.TextTitle = item.TextTitle;
                temp.TextChangeDate = item.TextChangeDate;
                temp.Hot = item.Hot;
                models.Add(temp);
            }
            models.Reverse();
            int pageSize = 4;//每页显示的文章数
            int pageNumber = (page ?? 1);
            return (models.ToPagedList(pageNumber, pageSize));
        }
    }
}
