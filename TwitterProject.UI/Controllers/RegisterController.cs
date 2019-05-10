using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterProject.Model.Option;
using TwitterProject.Service.Option;
using TwitterProject.UI.Models.VM;
using TwitterProject.Utility;

namespace TwitterProject.UI.Controllers
{
    public class RegisterController : Controller
    {
        AppUserService _appUserService;
        public RegisterController()
        {
            _appUserService = new AppUserService();
        }
                public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(AppUser data)//httppostfilebase:Server'a atılacak olan resmi barındıracak olan property-resim yükleme yolu
        {
            _appUserService.Add(data);
            return Redirect("/Account/Login");
        }

    }
}