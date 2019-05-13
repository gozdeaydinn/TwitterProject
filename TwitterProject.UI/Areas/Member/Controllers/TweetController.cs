using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterProject.Model.Option;
using TwitterProject.Service.Option;
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
        public JsonResult AddTweet(Tweet data, HttpPostedFileBase Image)
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

            data.AppUserID = _appUserService.FindByUserName(HttpContext.User.Identity.Name).ID;
            data.CreatedDate = DateTime.Now;

            bool isAdded = false;
            try
            {
                 _tweetService.Add(data);
                isAdded = true;
            }
            catch (Exception ex)
            {
                isAdded = false;
            }

            return Json(isAdded, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TweetList(string id)
        {

            //Guid tweetID = new Guid(id);
            Tweet tweet =_tweetService.GetDefault(x =>x.Status == Core.Enum.Status.Active).FirstOrDefault();

            return Json(new
            {
                AppUserImagePath = tweet.AppUser.UserImage,
                FirstName = tweet.AppUser.FirstName,
                LastName = tweet.AppUser.LastName,
                CreatedDate = tweet.CreatedDate.ToString(),
                Content = tweet.Content,
                TweetCount = _tweetService.GetDefault(x => x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated).Count()

                //CommentCount = _commentService.GetDefault(x => x.TweetID == tweetID && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated)).Count(),
                //LikeCount = _likeService.GetDefault(x => x.TweetID == tweetID && (x.Status == Core.Enum.Status.Active || x.Status == Core.Enum.Status.Updated)).Count(),
            }, JsonRequestBehavior.AllowGet);
        }

    }
}