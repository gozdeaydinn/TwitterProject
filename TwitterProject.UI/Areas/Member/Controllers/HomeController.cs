using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterProject.Service.Option;
using TwitterProject.UI.Areas.Member.Models.VM;

namespace TwitterProject.UI.Areas.Member.Controllers
{
    public class HomeController : Controller
    {
        TweetService _tweetService;
        AppUserService _appUserService;
        CommentService _commentService;
        LikeService _likeService;
        public HomeController()
        {
            _tweetService = new TweetService();
            _appUserService = new AppUserService();
            _commentService = new CommentService();
            _likeService = new LikeService();
        }
        public ActionResult Index()
        {
            TweetDetailVM model = new TweetDetailVM();

            model.Tweets = _tweetService.GetActive();
            
            foreach (var item in model.Tweets)
            {
                model.AppUser = _appUserService.GetById(item.AppUser.ID);
                model.Tweet = _tweetService.GetById(item.ID);
                model.Comments = _commentService.GetDefault(x => x.TweetID == item.ID && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated)).OrderByDescending(x => x.CreatedDate).Take(10).ToList();
                model.Likes = _likeService.GetDefault(x => x.TweetID == item.ID && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated));
                model.LikeCount = _likeService.GetDefault(x => x.TweetID == item.ID && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated)).Count();
                model.CommentCount = _commentService.GetDefault(x => x.TweetID == item.ID && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated)).Count();
            }
            return View(model);
        }
    }
}