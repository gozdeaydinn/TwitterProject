using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitterProject.UI.Models.VM
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "User Name Error!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Error!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email Error!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name Error!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Error!")]
        public string LastName { get; set; }
    }
}