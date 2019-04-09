using BlogBLL.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BlogBLL
{
	public class BlogSide:IBLL
    {
        private BlogDAL.BlogDAL repository = new BlogDAL.BlogDAL();

        public List<TextListsHot> GetsortByhots(int n)//博文根据热度排序
        {
            var hots = new List<TextListsHot>();
            var blog = repository.GetTextsAll();
            var list = blog.OrderByDescending(m => m.Hot).Take(n).ToList();
            foreach (var item in list)
            {
				var temp = new TextListsHot
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					Hot = item.Hot,
					Datemouth = item.TextChangeDate.ToString().Substring(0, 6)
				};
				hots.Add(temp);
            }
            return hots;
        }

        public List<TextListsHot> GetsortBytime(int n)//博文根据时间排序，参数n为获取的文章数量
        {
            var time_lists = new List<TextListsHot>();
            var blog = repository.GetTextsAll();
            var time_list = blog.OrderByDescending(m => m.TextChangeDate).Take(n).ToList();
            foreach (var item in time_list)
            {
				var temp = new TextListsHot
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					Hot = item.Hot,
					CategoryName = item.CategoryName,
					Datemouth = item.TextChangeDate.ToString().Substring(0, 6)
				};
				time_lists.Add(temp);
            }
            return time_lists;
        }

        public List<ShowComment> GetNewCommit(int n)//获取最新评论，参数n为获取的评论数量
        {
            var blog = repository.GetTextsAll();
            var commit = repository.GetCommentsAll();
            var Ctime_list = commit.OrderByDescending(m => m.CommentChangeDate).Take(n).ToList();
            var CommentList = new List<ShowComment>();
            var users = repository.GetUsersAll();
            foreach (var item in Ctime_list)
			{
				var textT = blog.Where(m => m.TextID == item.TextID).ToList();
				var NameC = users.Where(m => m.Account == item.Account).ToList();
				var tempC = new ShowComment
				{
					TextID = item.TextID,
					Content = item.CommentText,
					TextTitle = textT[0].TextTitle,
					Name = NameC[0].Name,
					Date = item.CommentChangeDate.ToString()
				};
				CommentList.Add(tempC);
            }
            return CommentList;
        }

        public List<string> GetCateString()//获取所有分类的名字
        {
            var templist = new List<string>();
            var blog = repository.GetTextsAll();
            var cate_list = blog.ToList();
            foreach (var item in cate_list)
            {
				var temp = new TextListsHot
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					Hot = item.Hot
				};
				if (!templist.Contains(item.CategoryName))
                {
                    templist.Add(item.CategoryName);
                }
                temp.Datemouth = item.TextChangeDate.ToString().Substring(0, 6);
            }
            return templist;
        }
        
        public List<ShowComment> GetTopCmtLst(int n)//获取评论排行榜
        {
            var blog = repository.GetTextsAll();
            
            if (blog.Count == 0)
                return null;
            List<ShowComment> TCL = new List<ShowComment>();//TCL= Top Comments List
            foreach (var item in blog)
            {
                TCL.Add(new ShowComment
				{
					TextID = item.TextID,
					TextTitle = item.TextTitle,
					CmtCount = repository.GetCommitsByTextID(item.TextID).Count()
				});
            }
            if (TCL.Count == 0)
                return null;
            TCL = TCL.OrderByDescending(c => c.CmtCount).ToList();
            n = n > TCL.Count ? TCL.Count : n;//避免文章过少时溢出
            for (int i = 0; i < n; i++)
            {
                TCL[i].Num = i + 1;
            }

            return TCL.Take(n).ToList();
        }
    }
}
