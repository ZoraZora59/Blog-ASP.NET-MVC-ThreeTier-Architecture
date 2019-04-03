using BlogBLL.ViewModels;
using BlogModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogBLL
{
    public class BlogSide:IBLL
    {
        private BlogDAL.BlogDAL repository = new BlogDAL.BlogDAL();

        public List<TextListsHot> GetsortByhots()
        {
            var hots = new List<TextListsHot>();
            var blog = repository.GetTextsAll();
            var list = blog.OrderByDescending(m => m.Hot).Take(5).ToList();
            foreach (var item in list)
            {
                var temp = new TextListsHot();
                temp.TextID = item.TextID;
                temp.TextTitle = item.TextTitle;
                temp.Hot = item.Hot;

                temp.Datemouth = item.TextChangeDate.ToString().Substring(0, 6);
                hots.Add(temp);
            }
            return hots;
        }

        public List<TextListsHot> GetsortBytime()
        {
            var time_lists = new List<TextListsHot>();
            var blog = repository.GetTextsAll();
            var time_list = blog.OrderByDescending(m => m.TextChangeDate).Take(2).ToList();
            foreach (var item in time_list)
            {
                var temp = new TextListsHot();
                temp.TextID = item.TextID;
                temp.TextTitle = item.TextTitle;
                temp.Hot = item.Hot;
                temp.CategoryName = item.CategoryName;
                temp.Datemouth = item.TextChangeDate.ToString().Substring(0, 6);
                time_lists.Add(temp);
            }
            return time_lists;
        }

        public List<ShowComment> GetNewCommit()
        {
            var blog = repository.GetTextsAll();
            var commit = repository.GetCommentsAll();
            var Ctime_list = commit.OrderByDescending(m => m.CommentChangeDate).Take(3).ToList();
            var CommentList = new List<ShowComment>();
            var users = repository.GetUsersAll();
            foreach (var item in Ctime_list)
            {
                var tempC = new ShowComment();
                var textT = blog.Where(m => m.TextID == item.TextID).ToList();
                var NameC = users.Where(m => m.Account == item.Account).ToList();
                tempC.TextID = item.TextID;
                tempC.Content = item.CommentText;
                tempC.TextTitle = textT[0].TextTitle;
                tempC.Name = NameC[0].Name;
                tempC.Date = item.CommentChangeDate.ToString();
                CommentList.Add(tempC);
            }
            return CommentList;
        }

        public List<string> GetCateString()
        {
            var templist = new List<string>();
            var blog = repository.GetTextsAll();
            var cate_list = blog.ToList();
            foreach (var item in cate_list)
            {
                var temp = new TextListsHot();
                temp.TextID = item.TextID;
                temp.TextTitle = item.TextTitle;
                temp.Hot = item.Hot;
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
            List<ShowComment> TCL = new List<ShowComment>();
            foreach (var item in blog)
            {
                TCL.Add(new ShowComment { TextID = item.TextID, TextTitle = item.TextTitle, CmtCount = repository.GetCommitsByTextID(item.TextID).Count() });
            }
            if (TCL.Count == 0)
                return null;
            TCL = TCL.OrderByDescending(c => c.CmtCount).ToList();
            n = n > TCL.Count ? TCL.Count : n;
            for (int i = 0; i < n; i++)
            {
                TCL[i].Num = i + 1;
            }

            return TCL.Take(n).ToList();
        }
    }
}
