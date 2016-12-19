using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { set; get; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
