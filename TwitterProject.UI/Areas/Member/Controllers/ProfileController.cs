using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterProject.Model.Option;
using TwitterProject.Service.Option;
using TwitterProject.UI.Areas.Member.Models.DTO;
using TwitterProject.UI.Areas.Member.Models.VM;
using TwitterProject.Utility;

namespace TwitterProject.UI.Areas.Member.Controllers
{
    public class ProfileController : Controller
    {
        AppUserService _appUserService;
        TweetService _tweetService;
        public ProfileController()
        {
            _appUserService = new AppUserService();
            _tweetService = new TweetService();
            
        }
        public ActionResult ProfileShow()
        {
            Guid userid = _appUserService.FindByUserName(User.Identity.Name).ID;
            TweetDetailVM model = new TweetDetailVM()
            {
                Tweets = _tweetService.GetDefault(x => x.AppUserID == userid &&(x.Status==Core.Enum.Status.Active)),
                AppUsers = _appUserService.GetDefault(x => x.ID == userid)
            };
            
            //List<AppUser> model = _appUserService.GetDefault(x=>x.ID==userid);
            return View(model);
        }
        public ActionResult Update(Guid id)
        {
            AppUser appuser = _appUserService.GetById(id);
            AppUserDTO model = new AppUserDTO();
            model.ID = appuser.ID;
            model.FirstName = appuser.FirstName;
            model.LastName = appuser.LastName;
            model.UserName = appuser.UserName;
            model.Password = appuser.Password;
            model.Bio = appuser.Bio;
            model.Email = appuser.Email;
            model.Birthdate = appuser.Birthdate;
            model.Gender = appuser.Gender;
            model.PhoneNumber = appuser.PhoneNumber;
            model.UserImage = appuser.UserImage;
            model.XSmallUserImage = appuser.XSmallUserImage;
            model.CruptedUserImage = appuser.CruptedUserImage;
            return View(model);

        }
        [HttpPost]
        public ActionResult Update(AppUserDTO data, HttpPostedFileBase Image)
        {
            List<string> UploadedImagePaths = new List<string>();

            UploadedImagePaths = ImageUploader.UploadSingleImage(ImageUploader.OriginalProfileImagePath, Image, 1);

            data.UserImage = UploadedImagePaths[0];


            AppUser appuser = _appUserService.GetById(data.ID);

            if (data.UserImage == "0" || data.UserImage == "1" || data.UserImage == "2")
            {

                if (appuser.UserImage == null || appuser.UserImage == ImageUploader.DefaultProfileImagePath)
                {
                    appuser.UserImage = ImageUploader.DefaultProfileImagePath;
                    appuser.XSmallUserImage = ImageUploader.DefaultXSmallProfileImage;
                    appuser.CruptedUserImage = ImageUploader.DefaulCruptedProfileImage;
                }
                else
                {
                    appuser.UserImage = appuser.UserImage;
                    appuser.XSmallUserImage = appuser.XSmallUserImage;
                    appuser.CruptedUserImage = appuser.CruptedUserImage;
                }

            }
            else
            {
                appuser.UserImage = UploadedImagePaths[0];
                appuser.XSmallUserImage = UploadedImagePaths[1];
                appuser.CruptedUserImage = UploadedImagePaths[2];
            }


            appuser.FirstName = data.FirstName;
            appuser.LastName = data.LastName;
            appuser.UserName = data.UserName;
            appuser.Password = data.Password;
            appuser.Email = data.Email;
            appuser.Gender = data.Gender;
            appuser.PhoneNumber = data.PhoneNumber;
            appuser.Birthdate = data.Birthdate;
            appuser.Bio = data.Bio;
            _appUserService.Update(appuser);

            return Redirect("/Member/Profile/ProfileShow");
        }
    }
}