using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogBLL.App_Code;
using BlogModel;
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

            //var hots = new List<TextListsHot>();
            var hots = side.GetsortByhots();
            //热度排序


            //最新博文，时间排序
            //var time_lists = new List<TextListsHot>();
            var time_lists = side.GetsortBytime();
            ViewBag.timesort = time_lists;
            

            //分类查找
            //var templist = new List<string>();
            var templist = side.GetCateString();
            ViewBag.categroyList = templist;


            //最新评论
            //var tempC = new ShowComment();
            var NewCommit = side.GetNewCommit();
            ViewBag.newestCom= NewCommit;



            ViewBag.TopComList = side.GetTopCmtLst(5);
            //设定评论排行榜的文章数量



            return View("~/Views/Shared/_Sidebar.cshtml", hots);
        }
    }
    
}