using System.Web.Mvc;
using BlogBLL.App_Code;
using BlogBLL.ViewModels;

namespace BlogRefactored.Controllers
{

	public class SideController : Controller
    {
        private BlogBLL.BlogSide side;
        public SideController(BlogBLL.BlogSide blogside)
        {
            this.side = blogside;
        }

        [ChildActionOnly]
        public ActionResult Sidebar(string searchSthing)
        {
            var model = new SerializeTool().DeSerialize<BlogConfig>();
            ViewBag.Config = model;
			
            var hots = side.GetsortByhots(5);
            //热度排序


            //最新博文，时间排序
            var time_lists = side.GetsortBytime(2);
            ViewBag.timesort = time_lists;
            

            //分类查找
            var templist = side.GetCateString();
            ViewBag.categroyList = templist;


            //最新评论
            var NewCommit = side.GetNewCommit(3);
            ViewBag.newestCom= NewCommit;


			//设定评论排行榜的文章数量
			ViewBag.TopComList = side.GetTopCmtLst(5);

			
            return View("~/Views/Shared/_Sidebar.cshtml", hots);
        }
    }
    
}