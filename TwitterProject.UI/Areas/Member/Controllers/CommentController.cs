using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterProject.Model.Option;
using TwitterProject.Service.Option;
using TwitterProject.UI.Areas.Member.Models.VM;

namespace TwitterProject.UI.Areas.Member.Controllers
{
    public class CommentController : Controller
    {
        CommentService _commentService;
        AppUserService _appUserService;
        LikeService _likeService;
        TweetService _tweetService;

        public CommentController()
        {
            _commentService = new CommentService();
            _appUserService = new AppUserService();
            _likeService = new LikeService();
            _tweetService = new TweetService();
        }
        public JsonResult AddComment(string userComment, Guid id)
        {
            Comment comment = new Comment();

            comment.AppUserID = _appUserService.FindByUserName(HttpContext.User.Identity.Name).ID;
            comment.TweetID = id;
            comment.Content = userComment;

            bool isAdded = false;
            try
            {
                _commentService.Add(comment);
                isAdded = true;
            }
            catch (Exception ex)
            {
                isAdded = false;
            }

            return Json(isAdded, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTweetComment(string id)
        {
            Guid tweetID = new Guid(id);

            Comment comment = _commentService.GetDefault(x => x.TweetID == tweetID && x.Status == Core.Enum.Status.Active).LastOrDefault();

           
            return Json(new
            {
                AppUserImagePath = comment.AppUser.UserImage,
                FirstName = comment.AppUser.FirstName,
                LastName = comment.AppUser.LastName,
                CreatedDate = comment.CreatedDate.ToString(),
                Content = comment.Content,
                CommentCount = _commentService.GetDefault(x => x.TweetID == tweetID && x.Status == Core.Enum.Status.Active).Count(),
                LikeCount = _likeService.GetDefault(x => x.TweetID == tweetID && x.Status == Core.Enum.Status.Active).Count(),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteComment(Guid id)
        {
            Guid userID = _appUserService.FindByUserName(HttpContext.User.Identity.Name).ID;
            Comment comment = _commentService.GetById(id);
            TweetDetailVM data = new TweetDetailVM();

            if (comment.AppUserID == userID)
            {
                _commentService.Remove(id);
                return Redirect("/Member/Tweet/Show/" + comment.TweetID);
            }
            else
            {
                return View();
            }
        }
    }
}