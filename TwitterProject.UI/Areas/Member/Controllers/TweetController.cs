using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterProject.Model.Option;
using TwitterProject.Service.Option;
using TwitterProject.UI.Areas.Member.Models.VM;
using TwitterProject.Utility;

namespace TwitterProject.UI.Areas.Member.Controllers
{
    public class TweetController : Controller
    {
        CommentService _commentService;
        AppUserService _appUserService;
        LikeService _likeService;
        TweetService _tweetService;
        public TweetController()
        {
            _commentService = new CommentService();
            _appUserService = new AppUserService();
            _likeService = new LikeService();
            _tweetService = new TweetService();
        }
        // GET: Member/Tweet
        [HttpPost]
        public ActionResult AddTweet(Tweet data, HttpPostedFileBase Image)
        {
            List<string> UploadedImagePaths = new List<string>();

            UploadedImagePaths = ImageUploader.UploadSingleImage(ImageUploader.OriginalProfileImagePath, Image, 1);

            data.ImagePath = UploadedImagePaths[0];

            if (data.ImagePath == "0" || data.ImagePath == "1" || data.ImagePath == "2")
            {
                data.ImagePath = ImageUploader.DefaultProfileImagePath;
                data.ImagePath = ImageUploader.DefaultXSmallProfileImage;
                data.ImagePath = ImageUploader.DefaulCruptedProfileImage;
            }
            else
            {
                data.ImagePath = UploadedImagePaths[1];
                data.ImagePath = UploadedImagePaths[2];
            }

            AppUser user = _appUserService.GetByDefault(x => x.UserName == User.Identity.Name);
            data.AppUserID = user.ID;
            data.CreatedDate = DateTime.Now;

            _tweetService.Add(data);
            return Redirect("/Member/Home/Index");
        }
        public ActionResult DeleteTweet(Guid id)
        {
            Tweet tweet = _tweetService.GetById(id);
            Guid userid = _appUserService.FindByUserName(HttpContext.User.Identity.Name).ID;
            if (tweet.AppUserID==userid)
            {
                _tweetService.Remove(id);
                return Redirect("/Member/Home/Index");
            }
            else
            {
                return Redirect("/Member/Home/Index");
            }
           
        }
        public ActionResult Show(Guid id)
        {
            TweetDetailVM model = new TweetDetailVM();
            model.Tweet = _tweetService.GetById(id);
            model.AppUser = _appUserService.GetById(model.Tweet.AppUser.ID);
            model.Comments = _commentService.GetDefault(x => x.TweetID == id && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated));
            model.LikeCount = _likeService.GetDefault(x => x.TweetID == id && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated)).Count;
            model.CommentCount = _commentService.GetDefault(x => x.TweetID == id && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated)).Count;
            model.Likes = _likeService.GetDefault(x => x.TweetID == id && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated));
        

            return View(model);

        }
    }
}