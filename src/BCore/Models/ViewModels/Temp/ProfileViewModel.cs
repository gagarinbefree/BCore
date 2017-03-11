using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Models.ViewModels
{
    public class ProfileViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { set; get; }

        public string ReturnUrl { get; set; }
    }
}
